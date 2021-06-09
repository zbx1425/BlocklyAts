using BlocklyAts.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlocklyAts.WebView {

    public class WinformBrowser : BaseBrowser {

        private WebBrowser browser;

        public override event EventHandler PageFinished;
        public override event PreviewKeyDownEventHandler KeyDown;
        public override event EventHandler<InteropReceivedEventArgs> InteropReceived;

        private const int FEATURE_LOCALMACHINE_LOCKDOWN = 8;
        private const int SET_FEATURE_ON_PROCESS = 0x00000002;

        [DllImport("urlmon.dll")]
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        static extern int CoInternetSetFeatureEnabled(int FeatureEntry, [MarshalAs(UnmanagedType.U4)] int dwFlags, bool fEnable);

        [ComVisible(true)]
        public class ScriptManager {

            public WinformBrowser Browser;

            public ScriptManager(WinformBrowser browser) {
                this.Browser = browser;
            }

            // This method can also be called from JavaScript.
            public void SendInterop(string message) {
                Browser.InteropReceived?.Invoke(Browser, new InteropReceivedEventArgs(message));
            }
        }

        public WinformBrowser(string url = "about:blank") {
            if (!PlatformFunction.IsMono) {
                try {
                    PlatformFunction.SetWebBrowserFeatures();
                    CoInternetSetFeatureEnabled(FEATURE_LOCALMACHINE_LOCKDOWN, SET_FEATURE_ON_PROCESS, false);
                } catch {
                    // No need to handle registry errors
                }
            }
            browser = new WebBrowser {
                IsWebBrowserContextMenuEnabled = false,
                AllowWebBrowserDrop = false,
                ScriptErrorsSuppressed = false,
                WebBrowserShortcutsEnabled = false
            };
            browser.DocumentCompleted += Browser_DocumentCompleted;
            browser.PreviewKeyDown += Browser_PreviewKeyDown;
            browser.ObjectForScripting = new ScriptManager(this);
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

# pragma warning disable 1998 // WebBrowser API calls has to be synchronous.
        public override async Task<object> InvokeScript(string script) {
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

        public override void ShowDevTools() {
            // MSHTML does not provide dev tools
        }
    }
}
