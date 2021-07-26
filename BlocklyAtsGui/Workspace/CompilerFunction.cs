using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using BlocklyAts.Host;
using System.Reflection;
using System.Reflection.Emit;

namespace BlocklyAts.Workspace {

    public static class CompilerFunction {

        public static readonly string libDir;

        public static readonly string CodeFunction, CodeOpenBve;

        static CompilerFunction() {
#if DEBUG
            libDir = Path.Combine(Path.GetDirectoryName(PlatformFunction.AppDir), "assets", "lib");
            if (!Directory.Exists(libDir)) libDir = Path.Combine(PlatformFunction.AppDir, "lib");
#else
            libDir = Path.Combine(PlatformFunction.AppDir, "lib");
#endif
            CodeFunction = File.ReadAllText(Path.Combine(libDir, "function.cs"));
            CodeOpenBve = File.ReadAllText(Path.Combine(libDir, "openbve.cs"));
        }

        public class CompileException : Exception {

            public IEnumerable<CompilerError> Errors { get; set; }

            public CompileException(System.Collections.CollectionBase errors) : base("Compile Error") {
                this.Errors = Enumerable.Cast<CompilerError>(errors);
            }
        }

        public enum Platform {
            OpenBve,
            WinDll32,
            WinDll64
        }

        private static void CopySectionTo(this Stream input, Stream output, int bytes) {
            byte[] buffer = new byte[32768];
            int read;
            while (bytes > 0 &&
                   (read = input.Read(buffer, 0, Math.Min(buffer.Length, bytes))) > 0) {
                output.Write(buffer, 0, read);
                bytes -= read;
            }
        }

        private static void ReplaceBytes(ref byte[] src, byte[] pattern, byte[] replace) {
            if (pattern.Length != replace.Length) throw new ArgumentException();
            int maxFirstCharSlot = src.Length - pattern.Length + 1;
            int j;
            for (int i = 0; i < maxFirstCharSlot; i++) {
                if (src[i] != pattern[0]) continue; //comp only first byte

                // found a match on first byte, it tries to match rest of the pattern
                for (j = pattern.Length - 1; j >= 1 && src[i + j] == pattern[j]; j--) ;
                if (j == 0) {
                    for (int k = 0; k < pattern.Length; k++) src[i + k] = replace[k];
                    i += pattern.Length;
                }
            }
        }

        public static string CombineCode(string script, Platform platform) {
            var guid = Guid.NewGuid();
            var sb = new StringBuilder();
            sb.Append("using System; using System.IO; using System.Collections.Generic; using System.Linq;");
            sb.Append("using System.Windows.Forms; ");
            if (platform == Platform.OpenBve) {
                sb.Append("using OpenBveApi.Runtime; using OpenBveApi.Colors;");
            }
            sb.Append("\n");
            sb.Append("namespace BlocklyAts {\n");
            sb.Append("\n\n// ----- Start of your program. -----\n\n");
            sb.Append(script);
            sb.Append("\n\n// ----- End of your program. You need not care about the following code. -----\n\n\n");
            sb.Append(CodeFunction);
            if (platform == Platform.OpenBve) {
                sb.Append(CodeOpenBve);
            }
            sb.Append("}\n");
            return sb.ToString();
        }

        public static void Compile(string script, string outputPath, Platform platform, bool includePDB) {
            const string defaultModuleName = "AtsCallConverter00000000000000000000000000000000";
            var randomModuleName = "AtsCallConverter" + Guid.NewGuid().ToString("N");
            // Prevent module name conflicts
            var tempOutputPath = Path.Combine(Path.GetDirectoryName(outputPath),
                "BlocklyAtsGenerated-" + Guid.NewGuid() + ".dll");
            var modifiedProxyDllPath = Path.Combine(Path.GetDirectoryName(outputPath), randomModuleName + ".dll");
            var sourceCode = CombineCode(script, platform);

            var settings = new Dictionary<string, string>() {
                { "CompilerVersion", "v4.0" }
            };
            string[] commonReferences = new string[] {
                "mscorlib.dll",
                "System.Core.dll",
                "System.dll",
                "Microsoft.CSharp.dll",
                "System.Windows.Forms.dll"
            };

            try {
                CSharpCodeProvider codeProvider = new CSharpCodeProvider(settings);
                CompilerParameters parameters = new CompilerParameters() {
                    IncludeDebugInformation = includePDB,
                    GenerateExecutable = false,
                    OutputAssembly = tempOutputPath,
                };
                if (!includePDB) parameters.CompilerOptions += "/optimize";

                foreach (string a in commonReferences) parameters.ReferencedAssemblies.Add(a);
                if (platform == Platform.OpenBve) {
                    parameters.ReferencedAssemblies.Add(Path.Combine(PlatformFunction.AppDir, "lib", "OpenBveApi.dll"));
                } else if (platform == Platform.WinDll32 || platform == Platform.WinDll64) {
                    var boilerplateFile = Path.Combine(
                        PlatformFunction.AppDir,
                        "lib",
                        platform == Platform.WinDll32 ? "AtsCallConverter_x86.dll" : "AtsCallConverter_x64.dll"
                    );
                    byte[] srcBytes = File.ReadAllBytes(boilerplateFile);
                    byte[] classNameBytes = Encoding.ASCII.GetBytes(randomModuleName);

                    // You need to use different module names for each plugin.
                    // Otherwise, the type references will mess up when multiple plugins are loaded by DetailManager.
                    ReplaceBytes(ref srcBytes, Encoding.ASCII.GetBytes(defaultModuleName),
                        Encoding.ASCII.GetBytes(randomModuleName));
                    ReplaceBytes(ref srcBytes, Encoding.Unicode.GetBytes(defaultModuleName),
                        Encoding.Unicode.GetBytes(randomModuleName));
                    File.WriteAllBytes(modifiedProxyDllPath, srcBytes);
                    parameters.ReferencedAssemblies.Add(modifiedProxyDllPath);
                }

                CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, sourceCode);
                if (results.Errors.HasErrors) {
                    throw new CompileException(results.Errors);
                }

                if (File.Exists(outputPath)) File.Delete(outputPath);
                if (platform == Platform.OpenBve) {
                    File.Copy(tempOutputPath, outputPath);
                    if (includePDB) {
                        var targetPdbFile = Path.ChangeExtension(outputPath, ".pdb");
                        if (File.Exists(targetPdbFile)) File.Delete(targetPdbFile);
                        File.Move(Path.ChangeExtension(tempOutputPath, ".pdb"), targetPdbFile);
                    }
                } else if (platform == Platform.WinDll32 || platform == Platform.WinDll64) {
                    var programData = new FileStream(tempOutputPath, FileMode.Open, FileAccess.Read);
                    var proxyStream = new FileStream(modifiedProxyDllPath, FileMode.Open, FileAccess.Read);
                    var outStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);

                    // Write Identifier and PE length to DOS stub
                    byte[] identifier = Encoding.UTF8.GetBytes("BATSNET1");
                    proxyStream.CopySectionTo(outStream, 0x6C - identifier.Length);
                    outStream.Write(identifier, 0, identifier.Length);
                    outStream.Write(BitConverter.GetBytes(proxyStream.Length), 0, 4);
                    outStream.Write(BitConverter.GetBytes(programData.Length), 0, 4);
                    proxyStream.Seek(8 + identifier.Length, SeekOrigin.Current);
                    proxyStream.CopyTo(outStream);
                    proxyStream.Close();

                    programData.CopyTo(outStream);
                    programData.Close();
                    if (includePDB) {
                        var pdbFilePath = Path.ChangeExtension(tempOutputPath, ".pdb");
                        var pdbStream = new FileStream(pdbFilePath, FileMode.Open, FileAccess.Read);
                        pdbStream.CopyTo(outStream);
                        pdbStream.Close();
                        File.Delete(pdbFilePath);
                    }
                    outStream.Close();
                }
            } finally {
                if (modifiedProxyDllPath != null) File.Delete(modifiedProxyDllPath);
                if (tempOutputPath != null) File.Delete(tempOutputPath);
            }
        }
    }
}
