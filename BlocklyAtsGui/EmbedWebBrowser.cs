using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlocklyATS {
    public interface IBrowser : IDisposable {
        Control GetControl();
        void BindTo(Control parent);
        void Reload();
        void Navigate(string url);
        event EventHandler PageFinished;
        event PreviewKeyDownEventHandler KeyDown;
        object InvokeScript(string script);
    }

    public class BuiltinBrowser : IBrowser {

        private WebBrowser browser;

        public event EventHandler PageFinished;

        public event PreviewKeyDownEventHandler KeyDown;

        private const int FEATURE_LOCALMACHINE_LOCKDOWN = 8;
        private const int SET_FEATURE_ON_PROCESS = 0x00000002;

        [DllImport("urlmon.dll")]
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        static extern int CoInternetSetFeatureEnabled(int FeatureEntry, [MarshalAs(UnmanagedType.U4)] int dwFlags, bool fEnable);

        public BuiltinBrowser(string url = "about:blank") {
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

        public Control GetControl() {
            return browser;
        }

        public void BindTo(Control parent) {
            browser.Dock = DockStyle.Fill;
            browser.Size = parent.ClientSize;
            parent.Controls.Add(browser);
            browser.BringToFront();
        }

        public object InvokeScript(string script) {
            return browser.Document.InvokeScript("eval", new object[] { script });
        }

        public void Navigate(string url) {
            browser.Navigate(new Uri(url));
        }

        public void Reload() {
            browser.Refresh();
        }

        public void Dispose() {
            ((IDisposable)browser).Dispose();
        }
    }

    public class CefBrowser : IBrowser {

        private CefSharp.WinForms.ChromiumWebBrowser browser;

        public event EventHandler PageFinished;

        public event PreviewKeyDownEventHandler KeyDown;

        public class KeyboardHandler : IKeyboardHandler {

            private CefBrowser parent;

            public KeyboardHandler(CefBrowser browser) {
                this.parent = browser;
            }

            public bool OnKeyEvent(IWebBrowser chromiumWebBrowser, CefSharp.IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey) {
                if (type == KeyType.KeyUp && Enum.IsDefined(typeof(Keys), windowsKeyCode)) {
                    parent.KeyDown?.Invoke(browser, new PreviewKeyDownEventArgs((Keys)windowsKeyCode));
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

        public Control GetControl() {
            return browser;
        }

        public void BindTo(Control parent) {
            browser.Dock = DockStyle.Fill;
            browser.Size = parent.ClientSize;
            parent.Controls.Add(browser);
            browser.BringToFront();
        }

        public object InvokeScript(string script) {
            var task = browser.GetBrowser().MainFrame.EvaluateScriptAsync(script);
            task.Wait();
            if (task.Result.Success) {
                return task.Result.Result;
            } else {
                throw new Exception(task.Result.Message);
            }
        }

        public void Navigate(string url) {
            browser.Load(url);
        }

        public void Reload() {
            browser.Load(browser.GetBrowser().MainFrame.Url);
        }

        public void ShowDevTools() {
            browser.ShowDevTools();
        }

        public void Dispose() {
            ((IDisposable)browser).Dispose();
            Cef.Shutdown();
        }
    }
}
