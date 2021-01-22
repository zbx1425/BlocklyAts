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

        public static string appDir = Path.GetDirectoryName(Application.ExecutablePath);

        public static Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default(CancellationToken)) {
            if (process.HasExited) return Task.CompletedTask;

            var tcs = new TaskCompletionSource<object>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => tcs.TrySetResult(null);
            if (cancellationToken != default(CancellationToken))
                cancellationToken.Register(() => tcs.SetCanceled());

            return process.HasExited ? Task.CompletedTask : tcs.Task;
        }

        public static async Task CompileLua(string script, string outputPath, string arch) {
            /*var proc = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = Path.Combine(appDir, "bin", "luac" + arch + ".exe"),
                    Arguments = "-o - -- -",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            await proc.StandardInput.WriteAsync(luaScript);
            proc.StandardInput.Close();
            await proc.WaitForExitAsync();
            if (proc.ExitCode != 0) {
                string err = proc.StandardError.ReadToEnd();
                throw new ExternalException(err);
            }
            var luaByteStream = new MemoryStream();*/

            var boilerplateStream = new FileStream(
                Path.Combine(appDir, "bin", "batswinapi_" + arch + ".dll"), 
                FileMode.Open, FileAccess.Read
            );
            var outStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            await boilerplateStream.CopyToAsync(outStream);
            //await proc.StandardOutput.BaseStream.CopyToAsync(outStream);
            byte[] confusion = { 0x11, 0x45, 0x14, 0x19, 0x19, 0x81, 0x14, 0x25 };
            byte[] srcCode = Encoding.UTF8.GetBytes(script);
            for (int i = 0; i < srcCode.Length; i++) srcCode[i] ^= confusion[i % 8];
            await outStream.WriteAsync(srcCode, 0, srcCode.Length);
            boilerplateStream.Close();
            outStream.Close();

            /*var luabin = "lua54_" + arch + ".dll";
            if (!arch.EndsWith("_static")) {
                File.Copy(
                    Path.Combine(appDir, "bin", luabin),
                    Path.Combine(Path.GetDirectoryName(outputPath), luabin),
                    true
                );
            } else if (File.Exists(Path.Combine(Path.GetDirectoryName(outputPath), luabin))) {
                File.Delete(Path.Combine(Path.GetDirectoryName(outputPath), luabin));
            }*/
        }

        public static void CompileCSharp(string script, string outputPath) {
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
                "System", "System.Core", "System.Data", "mscorlib",
                "Microsoft.CSharp", "System.Windows.Forms",
                Path.Combine(appDir, "bin", "OpenBveApi")
            };

            foreach (string a in assemblies) {
                parameters.ReferencedAssemblies.Add(a + ".dll");
            }
            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, script);
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
