using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlocklyATS {

    public class WinformBrowser : BaseBrowser {

        private WebBrowser browser;

        public override event EventHandler PageFinished;

        public override event PreviewKeyDownEventHandler KeyDown;

        private const int FEATURE_LOCALMACHINE_LOCKDOWN = 8;
        private const int SET_FEATURE_ON_PROCESS = 0x00000002;

        [DllImport("urlmon.dll")]
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        static extern int CoInternetSetFeatureEnabled(int FeatureEntry, [MarshalAs(UnmanagedType.U4)] int dwFlags, bool fEnable);

        public WinformBrowser(string url = "about:blank") {
            try {
                PlatformFunction.SetWebBrowserFeatures();
                CoInternetSetFeatureEnabled(FEATURE_LOCALMACHINE_LOCKDOWN, SET_FEATURE_ON_PROCESS, false);
            } catch {
                // No need to handle registry errors
            }
            browser = new WebBrowser {
                IsWebBrowserContextMenuEnabled = false,
                AllowWebBrowserDrop = false,
                //ScriptErrorsSuppressed = true,
                WebBrowserShortcutsEnabled = false
            };
            browser.DocumentCompleted += Browser_DocumentCompleted;
            browser.PreviewKeyDown += Browser_PreviewKeyDown;
            if (url != "about:blank") browser.Navigate(new Uri(url));
        }

        private void Browser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            KeyDown?.Invoke(sender, e);
        }

        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
            PageFinished?.Invoke(sender, e);
        }

        public override Control GetControl() {
            return browser;
        }

        public override object InvokeScript(string script) {
            return browser.Document.InvokeScript("eval", new object[] { script });
        }

        public override void Navigate(string url) {
            browser.Navigate(new Uri(url));
        }

        public override void Reload() {
            browser.Refresh();
        }

        public override void Dispose() {
            ((IDisposable)browser).Dispose();
        }
    }
}
