using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BlocklyAts {
    partial class WebView2Browser {

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        private static void TryLoadWebview2Loader() {
            var FLoaderLoaded = typeof(CoreWebView2Environment)
                .GetField("webView2LoaderLoaded", BindingFlags.Static | BindingFlags.NonPublic);
            if (!(bool)FLoaderLoaded.GetValue(null)) {
                string frameworkDescription = RuntimeInformation.FrameworkDescription;
                if (frameworkDescription.StartsWith(".NET Framework")) {
                    string localPath = new Uri(typeof(CoreWebView2Environment).Assembly.CodeBase).LocalPath;
                    string directoryName = Path.GetDirectoryName(localPath);
                    string arch;
                    switch (RuntimeInformation.ProcessArchitecture) {
                        case Architecture.X86:
                            arch = "x86";
                            break;
                        case Architecture.X64:
                            arch = "x64";
                            break;
                        case Architecture.Arm64:
                            arch = "arm64";
                            break;
                        default:
                            throw new NotSupportedException(
                                string.Format("{0} bit WebView2Loader.dll is not supported", RuntimeInformation.ProcessArchitecture)
                            );
                    }
                    string dllName = "WebView2Loader." + arch + ".dll";
                    if (File.Exists(dllName)) {
                        File.Copy(dllName, "WebView2Loader.dll", true);
                        if (LoadLibrary("WebView2Loader.dll") == IntPtr.Zero) {
                            int hrforLastWin32Error = Marshal.GetHRForLastWin32Error();
                            Marshal.ThrowExceptionForHR(hrforLastWin32Error);
                        } else {
                            FLoaderLoaded.SetValue(null, true);
                        }
                    }
                }
            }
        }
    }
}
