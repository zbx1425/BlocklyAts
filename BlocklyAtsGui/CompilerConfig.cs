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
        static extern int MsiGetProductInfo(string product, string property, [Out] StringBuilder valueBuf, ref int len);
        [DllImport("msi.dll", SetLastError=true)]
        static extern uint MsiOpenDatabase(string szDatabasePath, int szPersist, out IntPtr phDatabase);
        [DllImport("msi.dll", CharSet = CharSet.Unicode)]
        static extern int MsiDatabaseOpenView(IntPtr hDatabase, [MarshalAs(UnmanagedType.LPWStr)] string szQuery, out IntPtr phView);
        [DllImport("msi.dll", CharSet = CharSet.Unicode)]
        static extern int MsiViewExecute(IntPtr hView, IntPtr hRecord);
        [DllImport("msi.dll", CharSet = CharSet.Unicode)]
        static extern uint MsiViewFetch(IntPtr hView, out IntPtr hRecord);
        [DllImport("msi.dll")]
        static extern int MsiViewClose(IntPtr hView);
        [DllImport("msi.dll", CharSet = CharSet.Unicode)]
        static extern int MsiRecordGetString(IntPtr hRecord, int iField, [Out] StringBuilder szValueBuf, ref int pcchValueBuf);
        [DllImport("msi.dll", CharSet = CharSet.Unicode)]
        static extern uint MsiGetComponentPath(string szProduct, string szComponent, [Out] StringBuilder lpPathBuf, ref int pcchBuf);


        private string GetMsiBasePath(string AppID) {
            string possiblePath = null;
            int szDbPath = 512;
            var bufDbPath = new StringBuilder(szDbPath);
            MsiGetProductInfo(AppID, "LocalPackage", bufDbPath, ref szDbPath);
            MsiOpenDatabase(bufDbPath.ToString(), 0, out IntPtr hDatabase);
            MsiDatabaseOpenView(hDatabase, "SELECT ComponentId FROM `Component`", out IntPtr hView);
            MsiViewExecute(hView, IntPtr.Zero);
            MsiViewFetch(hView, out IntPtr hRecord);
            while (hRecord != IntPtr.Zero) {
                int szComponentGuid = 512;
                var bufComponentGuid = new StringBuilder(szComponentGuid);
                MsiRecordGetString(hRecord, 1, bufComponentGuid, ref szComponentGuid);
                int szInstallPath = 512;
                var bufInstallPath = new StringBuilder(szInstallPath);
                var installState = MsiGetComponentPath(AppID, bufComponentGuid.ToString(), bufInstallPath, ref szInstallPath);
                if ((installState == 3 || installState == 4) && // Component is installed
                    (szInstallPath > 1 && bufInstallPath[0] != '0' && bufInstallPath[0] != '2')) { // Shouldn't be a Registry Key
                    if (possiblePath == null || possiblePath.Length > bufInstallPath.Length) possiblePath = bufInstallPath.ToString();
                }
                MsiViewFetch(hView, out hRecord);
            }
            MsiViewClose(hView);
            return possiblePath;
        }
#endif

        public Process GetGameProcess() {
            var path = GamePath;

            // OpenBVE
            if (string.IsNullOrEmpty(path)) {
                if (PlatformFunction.IsWindows()) {
                    const string InnoAppID = "{D617A45D-C2F6-44D1-A85C-CA7FFA91F7FC}_is1";
                    var msiUninstallKeys = new List<RegistryKey>();
                    var msiUninstallParentNames = new string[] {
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\",
                        @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\",
                    };
                    foreach (var msiUninstallParentName in msiUninstallParentNames) {
                        try {
                            msiUninstallKeys.Add(Registry.CurrentUser.OpenSubKey(msiUninstallParentName + InnoAppID));
                            msiUninstallKeys.Add(Registry.LocalMachine.OpenSubKey(msiUninstallParentName + InnoAppID));
                        } catch { }
                    }
                    string[] InstallLocations = msiUninstallKeys
                        .Select(regKey => regKey != null ? regKey.GetValue("InstallLocation", null) : null)
                        .OfType<string>().Distinct().ToArray();
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
            // BVE6
            if (string.IsNullOrEmpty(path) && PlatformFunction.IsWindows()) {
                const string Bve6ProductID = "{AB8616E0-A471-4261-9563-FE411A2A245B}";
                var basePath = GetMsiBasePath(Bve6ProductID);
                if (basePath != null) path = Path.Combine(basePath, "BveTs.exe");
            }

            // BVE5
            if (string.IsNullOrEmpty(path) && PlatformFunction.IsWindows()) {
                const string Bve5ProductID = "{D38EB8AB-0772-473D-9443-9B2149E4F13D}";
                var basePath = GetMsiBasePath(Bve5ProductID);
                if (basePath != null) path = Path.Combine(basePath, "BveTs.exe");
            }
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
