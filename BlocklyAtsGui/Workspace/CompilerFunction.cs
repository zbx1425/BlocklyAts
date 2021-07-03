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
            var sourceCode = CombineCode(script, platform);

            var settings = new Dictionary<string, string>() {
                { "CompilerVersion", "v4.0" }
            };
            CSharpCodeProvider codeProvider = new CSharpCodeProvider(settings);
            CompilerParameters parameters = new CompilerParameters() {
                IncludeDebugInformation = includePDB,
                GenerateExecutable = false,
                OutputAssembly = outputPath,
            };
            if (!includePDB) parameters.CompilerOptions += "/optimize ";

            string[] commonReferences = new string[] {
                "mscorlib.dll",
                "System.Core.dll",
                "System.dll",
                "Microsoft.CSharp.dll",
                "System.Windows.Forms.dll"
            };
            foreach (string a in commonReferences) parameters.ReferencedAssemblies.Add(a);
            if (platform == Platform.OpenBve) {
                parameters.ReferencedAssemblies.Add(Path.Combine(PlatformFunction.AppDir, "lib", "OpenBveApi.dll"));
            }

            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, sourceCode);
            if (results.Errors.HasErrors) {
                throw new CompileException(results.Errors);
            }

            if (platform == Platform.WinDll32 || platform == Platform.WinDll64) {
                var programData = File.ReadAllBytes(outputPath);
                var boilerplateFile = Path.Combine(
                    PlatformFunction.AppDir,
                    "lib",
                    platform == Platform.WinDll32 ? "AtsCallConverter_x86.dll" : "AtsCallConverter_x64.dll"
                );
                var boilerplateStream = new FileStream(boilerplateFile, FileMode.Open, FileAccess.Read);
                var outStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);

                // Write Identifier and PE length to DOS stub
                byte[] identifier = Encoding.UTF8.GetBytes("BATSNET1");
                boilerplateStream.CopySectionTo(outStream, 0x6C - identifier.Length);
                outStream.Write(identifier, 0, identifier.Length);
                outStream.Write(BitConverter.GetBytes(boilerplateStream.Length), 0, 4);
                outStream.Write(BitConverter.GetBytes(programData.Length), 0, 4);
                boilerplateStream.Seek(8 + identifier.Length, SeekOrigin.Current);
                boilerplateStream.CopyTo(outStream);
                boilerplateStream.Close();

                outStream.Write(programData, 0, programData.Length);
                if (includePDB) {
                    var pdbFilePath = Path.ChangeExtension(outputPath, ".pdb");
                    var pdbStream = new FileStream(pdbFilePath, FileMode.Open, FileAccess.Read);
                    pdbStream.CopyTo(outStream);
                    pdbStream.Close();
                    File.Delete(pdbFilePath);
                }
                outStream.Close();
            }
        }
    }
}
