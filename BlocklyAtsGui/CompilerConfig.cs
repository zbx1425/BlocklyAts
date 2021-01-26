using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlocklyAts {
    
    public class CompilerConfig {

        public bool ShouldCompileAnyCpu { get; set; }

        public bool ShouldCompilex86 { get; set; }

        public bool ShouldCompilex64 { get; set; }

        public string CompilePathAnyCpu { get; set; }

        public string CompilePathx86 { get; set; }

        public string CompilePathx64 { get; set; }

        public string GamePath { get; set; }

        public string GameArgs { get; set; }

        public CompilerConfig Clone() {
            return (CompilerConfig)this.MemberwiseClone();
        }

#if !MONO
        [DllImport("msi.dll", CharSet = CharSet.Unicode)]
        private static extern Int32 MsiGetProductInfo(string product, string property, [Out] StringBuilder valueBuf, ref Int32 len);
#endif

        public Process GetGameProcess() {
            var path = GamePath;

            // OpenBVE
            if (string.IsNullOrEmpty(path)) {
                if (PlatformFunction.IsWindows()) {
                    const string InnoAppID = "{D617A45D-C2F6-44D1-A85C-CA7FFA91F7FC}_is1";
                    string[] InstallLocations = new RegistryKey[] {
                        Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + InnoAppID),
                        Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + InnoAppID),
                        Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + InnoAppID),
                        Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + InnoAppID),
                    }.Select(regKey => regKey != null ? regKey.GetValue("InstallLocation", null) : null).OfType<string>().Distinct().ToArray();
                    foreach (string location in InstallLocations) {
                        string assemblyFile = Path.Combine(location, "OpenBve.exe");
                        if (!File.Exists(assemblyFile)) continue;
                        path = assemblyFile;
                        break;
                    }
                } else {
                    // TODO: Support them
                }
            }

#if !MONO
            // BVE5
            /*if (string.IsNullOrEmpty(path) && PlatformFunction.IsWindows()) {
                Int32 len = 512;
                StringBuilder builder = new StringBuilder(len);
                MsiGetProductInfo("{D38EB8AB-0772-473D-9443-9B2149E4F13D}", "LocalPackage", builder, ref len);
                string str = builder.ToString();
            }*/
#endif

            if (string.IsNullOrEmpty(path) || !File.Exists(path)) {
                return null;
            }
            return new Process() {
                StartInfo = new ProcessStartInfo {
                    FileName = path,
                    UseShellExecute = false,
                    Arguments = GameArgs
                }
            };
        }
    }
}
