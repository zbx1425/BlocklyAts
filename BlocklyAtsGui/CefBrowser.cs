#if !MONO
using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlocklyATS {

    public class CefBrowser : BaseBrowser {

        private CefSharp.WinForms.ChromiumWebBrowser browser;

        public override event EventHandler PageFinished;

        public override event PreviewKeyDownEventHandler KeyDown;

        public class KeyboardHandler : IKeyboardHandler {

            private CefBrowser parent;

            public KeyboardHandler(CefBrowser browser) {
                this.parent = browser;
            }

            public bool OnKeyEvent(IWebBrowser chromiumWebBrowser, CefSharp.IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey) {
                if (type == KeyType.KeyUp && Enum.IsDefined(typeof(Keys), windowsKeyCode)) {
                    parent.browser.BeginInvoke((Action)(() => parent.KeyDown?.Invoke(browser, new PreviewKeyDownEventArgs((Keys)windowsKeyCode))));
                }
                return false;
            }

            public bool OnPreKeyEvent(IWebBrowser chromiumWebBrowser, CefSharp.IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut) {
                return false;
            }
        }

        public CefBrowser(string url = "about:blank") {
            if (!Cef.IsInitialized) {
                var cefSettings = new CefSettings() {
                    BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                        Environment.Is64BitProcess ? "x64" : "x86", "CefSharp.BrowserSubprocess.exe"),
                    CefCommandLineArgs = { ["disable-gpu-shader-disk-cache"] = "1" },
                    LogSeverity = LogSeverity.Disable
                };
                Cef.Initialize(cefSettings);
            }
            browser = new ChromiumWebBrowser(url);
            var browserSettings = new BrowserSettings {
                FileAccessFromFileUrls = CefState.Enabled,
                UniversalAccessFromFileUrls = CefState.Enabled,
            };
            browser.BrowserSettings = browserSettings;
            browser.LoadingStateChanged += Browser_LoadingStateChanged;
            browser.KeyboardHandler = new KeyboardHandler(this);
        }

        private void Browser_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e) {
            if (e.IsLoading == false) PageFinished?.Invoke(this, EventArgs.Empty);
        }

        public override Control GetControl() {
            return browser;
        }

        public override object InvokeScript(string script) {
            var task = browser.GetBrowser().MainFrame.EvaluateScriptAsync(script);
            task.Wait();
            if (task.Result.Success) {
                return task.Result.Result;
            } else {
                throw new Exception(task.Result.Message);
            }
        }

        public override void Navigate(string url) {
            browser.Load(url);
        }

        public override void Reload() {
            browser.Load(browser.GetBrowser().MainFrame.Url);
        }

        public override void Dispose() {
            ((IDisposable)browser).Dispose();
            Cef.Shutdown();
        }
        
        public void ShowDevTools() {
            browser.ShowDevTools();
        }
    }
}
#endif