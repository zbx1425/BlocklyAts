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

namespace BlocklyAts.Workspace {

    static class CompilerFunction {

        public static readonly string libDir;
        public static readonly string BoilerplateStartMarker = "Start of BlocklyAts boilerplate code.";

        public static Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default(CancellationToken)) {
            if (process.HasExited) return Task.CompletedTask;

            var tcs = new TaskCompletionSource<object>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => tcs.TrySetResult(null);
            if (cancellationToken != default(CancellationToken))
                cancellationToken.Register(() => tcs.SetCanceled());

            return process.HasExited ? Task.CompletedTask : tcs.Task;
        }

        public static readonly string CodeFunction, CodeOpenBve;

        static CompilerFunction() {
#if DEBUG
            libDir = Path.Combine(Path.GetDirectoryName(PlatformFunction.AppDir), "assets", "lib");
            if (!Directory.Exists(libDir)) libDir = Path.Combine(PlatformFunction.AppDir, "lib");
#else
            libDir = Path.Combine(appDir, "lib");
#endif
            CodeFunction = File.ReadAllText(Path.Combine(libDir, "function.cs"));
            CodeOpenBve = File.ReadAllText(Path.Combine(libDir, "openbve.cs"));
        }

        public static string CombineCodeForCSharp(string script, bool includeOpenBve) {
            var sb = new StringBuilder();
            sb.Append("using System; using System.IO; using System.Collections.Generic; using System.Windows.Forms; ");
            if (includeOpenBve) sb.Append("using OpenBveApi.Runtime; "); else sb.Append("using BlocklyAts; ");
            sb.Append("\n");
            sb.Append("namespace BlocklyAts {\n");
            sb.Append("\n\n// ----- Start of your program. -----\n\n");
            sb.Append(script);
            sb.Append("\n\n// ----- End of your program. -----\n\n\n");
            sb.Append(CodeFunction);
            if (includeOpenBve) sb.Append(CodeOpenBve);
            sb.Append("}\n");
            return sb.ToString();
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

        public static void CompileCSharpOpenBve(string script, string outputPath, bool includePDB) {
            var sourceCode = CombineCodeForCSharp(script, true);
            var settings = new Dictionary<string, string>() {
                { "CompilerVersion", "v4.0" }
            };
            CSharpCodeProvider codeProvider = new CSharpCodeProvider(settings);
            CompilerParameters parameters = new CompilerParameters() {
                IncludeDebugInformation = includePDB,
                GenerateExecutable = false,
                OutputAssembly = outputPath
            };
            if (!includePDB) parameters.CompilerOptions = "/optimize";
            string[] assemblies = {
                "mscorlib.dll",
                "System.Core.dll",
                "System.dll",
                "Microsoft.CSharp.dll",
                "System.Windows.Forms.dll",
                Path.Combine(PlatformFunction.AppDir, "lib", "OpenBveApi.dll")
            };

            foreach (string a in assemblies) {
                parameters.ReferencedAssemblies.Add(a);
            }
            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, sourceCode);
            if (results.Errors.HasErrors) {
                StringBuilder sb = new StringBuilder();
                foreach (CompilerError error in results.Errors) {
                    sb.AppendLine(error.ToString());
                }
                throw new Exception(sb.ToString());
            }
        }

        public static void CompileCSharpUnmanaged(string script, string outputPath, string arch, bool includePDB) {
            var sourceCode = CombineCodeForCSharp(script, false);

            var boilerplateFile = Path.Combine(PlatformFunction.AppDir, "lib", "batsdllexport_" + arch + ".dll");
            var boilerplateStream = new FileStream(boilerplateFile, FileMode.Open, FileAccess.Read);
            var outStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);

            var dllFilePath = Path.Combine(Path.GetTempPath(), "BlocklyAts_Temp_" + Guid.NewGuid().ToString("N") + ".dll");
            var pdbFilePath = Path.ChangeExtension(dllFilePath, ".pdb");

            var settings = new Dictionary<string, string>() {
                { "CompilerVersion", "v4.0" }
            };
            CSharpCodeProvider codeProvider = new CSharpCodeProvider(settings);
            CompilerParameters parameters = new CompilerParameters() {
                IncludeDebugInformation = includePDB,
                GenerateExecutable = false,
                OutputAssembly = dllFilePath,
            };
            if (!includePDB) parameters.CompilerOptions = "/optimize";
            string[] assemblies = {
                "mscorlib.dll",
                "System.Core.dll",
                "Microsoft.CSharp.dll",
                "System.Windows.Forms.dll",
                boilerplateFile
            };

            foreach (string a in assemblies) {
                parameters.ReferencedAssemblies.Add(a);
            }
            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, sourceCode);
            if (results.Errors.HasErrors) {
                StringBuilder sb = new StringBuilder();
                foreach (CompilerError error in results.Errors) {
                    sb.AppendLine(error.ToString());
                }
                File.Delete(dllFilePath);
                boilerplateStream.Close();
                outStream.Close();
                throw new Exception(sb.ToString());
            }

            var programStream = new FileStream(dllFilePath, FileMode.Open, FileAccess.Read);

            // Write Identifier and PE length to DOS stub
            byte[] identifier = Encoding.UTF8.GetBytes("BATSNET1");
            boilerplateStream.CopySectionTo(outStream, 0x6C - identifier.Length);
            outStream.Write(identifier, 0, identifier.Length);
            outStream.Write(BitConverter.GetBytes(boilerplateStream.Length), 0, 4);
            outStream.Write(BitConverter.GetBytes(programStream.Length), 0, 4);
            boilerplateStream.Seek(8 + identifier.Length, SeekOrigin.Current);
            boilerplateStream.CopyTo(outStream);
            boilerplateStream.Close();

            programStream.CopyTo(outStream);
            programStream.Close();

            if (includePDB) {
                var pdbStream = new FileStream(pdbFilePath, FileMode.Open, FileAccess.Read);
                pdbStream.CopyTo(outStream);
                pdbStream.Close();
                File.Delete(pdbFilePath);
            }
            File.Delete(dllFilePath);
            outStream.Close();
        }
    }
}
