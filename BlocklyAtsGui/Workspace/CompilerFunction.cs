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

namespace BlocklyAts {

    static class CompilerFunction {

        public static readonly string appDir = Path.GetDirectoryName(Application.ExecutablePath);
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

        public static readonly string BoilerplateLua, BoilerplateCSharp;

        static CompilerFunction() {
#if DEBUG
            libDir = Path.Combine(Path.GetDirectoryName(appDir), "assets", "lib");
            if (!Directory.Exists(libDir)) libDir = Path.Combine(appDir, "lib");
#else
            libDir = Path.Combine(appDir, "lib");
#endif
            BoilerplateLua = File.ReadAllText(Path.Combine(libDir, "boilerplate.lua"));
            BoilerplateCSharp = File.ReadAllText(Path.Combine(libDir, "boilerplate.cs"));
        }

        public static string CombineCode(string boilerplate, string script) {
            int insertPosition = boilerplate.IndexOf(BoilerplateStartMarker) - 3;
            return (boilerplate.Substring(0, insertPosition).Trim()
                + "\n"
                + script.Replace("\r\n", "\n").Trim() 
                + "\n"
                + boilerplate.Substring(insertPosition)).Trim();
        }

        public static async Task CompileLua(string script, string outputPath, string arch) {
            var sourceCode = CombineCode(BoilerplateLua, script);

            var boilerplateStream = new FileStream(
                Path.Combine(appDir, "lib", "batswinapi_" + arch + ".dll"), 
                FileMode.Open, FileAccess.Read
            );
            var outStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            await boilerplateStream.CopyToAsync(outStream);

            byte[] confusion = { 0x11, 0x45, 0x14, 0x19, 0x19, 0x81, 0x14, 0x25 };
            byte[] srcCode = Encoding.UTF8.GetBytes(sourceCode);
            for (int i = 0; i < srcCode.Length; i++) srcCode[i] ^= confusion[i % 8];
            await outStream.WriteAsync(srcCode, 0, srcCode.Length);
            boilerplateStream.Close();
            outStream.Close();
        }

        public static void CompileCSharp(string script, string outputPath) {
            var sourceCode = CombineCode(BoilerplateCSharp, script);
            var settings = new Dictionary<string, string>() {
                { "CompilerVersion", "v4.0" }
            };
            CSharpCodeProvider codeProvider = new CSharpCodeProvider(settings);
            CompilerParameters parameters = new CompilerParameters() {
                IncludeDebugInformation = false,
                GenerateExecutable = false,
                CompilerOptions = "/optimize",
                OutputAssembly = outputPath
            };
            string[] assemblies = {
                "mscorlib.dll",
                "System.Core.dll",
                "Microsoft.CSharp.dll",
                "System.Windows.Forms.dll",
                Path.Combine(appDir, "lib", "OpenBveApi.dll")
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
    }
}
