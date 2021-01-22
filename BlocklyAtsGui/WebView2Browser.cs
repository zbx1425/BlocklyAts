using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace BlocklyAts {
    class WebView2Browser : BaseBrowser {
        public override event EventHandler PageFinished;
        public override event PreviewKeyDownEventHandler KeyDown;

        private CoreWebView2Environment environment;
        private WebView2 browser;

        public WebView2Browser(string url = "about:blank") {

            browser = new WebView2();
            browser.PreviewKeyDown += Browser_PreviewKeyDown;
            browser.NavigationCompleted += Browser_DocumentCompleted;

            var createTask = CoreWebView2Environment.CreateAsync();
            createTask.Wait();
            environment = createTask.Result;
            browser.CoreWebView2Ready += (sender, e) => {
                browser.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
                browser.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;
                browser.CoreWebView2.Settings.IsZoomControlEnabled = false;
                browser.CoreWebView2.Settings.AreDevToolsEnabled = false;
                browser.CoreWebView2.Settings.IsStatusBarEnabled = false;
                browser.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            };
            browser.EnsureCoreWebView2Async(environment);

            if (url[1] == ':' && url[2] == '\\') {
                // URLParser says file urls cannot have query param. Make them happy.
                browser.Source = new Uri("http://make.urlparser.happy/" + url);
            } else {
                browser.Source = new Uri(url);
            }
        }

        private void CoreWebView2_WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e) {
            string path;
            if (e.Request.Uri.StartsWith("file:///")) {
                path = e.Request.Uri.Substring(8).Replace('/', '\\');
                if (path.Contains("%3F")) path = path.Substring(0, path.IndexOf("%3F"));
            } else if (e.Request.Uri.StartsWith("http://make.urlparser.happy/")) {
                path = e.Request.Uri.Substring(28).Replace('/', '\\');
                if (path.Contains("?")) path = path.Substring(0, path.IndexOf("?"));
            } else {
                return;
            }
            var extension = Path.GetExtension(path).ToLowerInvariant();
            var mimeMap = new Dictionary<string, string>() {
                { ".html", "text/html;charset=utf-8" },
                { ".htm", "text/html;charset=utf-8" },
                { ".js", "text/javascript;charset=utf-8" },
                { ".css", "text/css;charset=utf-8" }
            };
            if (File.Exists(path)) {
                e.Response = environment.CreateWebResourceResponse(
                    new MemoryStream(File.ReadAllBytes(path)), 
                    200, "OK", "Content-Type: " + 
                        (mimeMap.TryGetValue(extension, out string mimeType) ? mimeType : "application/octet-stream"));
            } else {
                e.Response = environment.CreateWebResourceResponse(
                    new MemoryStream(Encoding.UTF8.GetBytes("404 Not Found")),
                    404, "Not Found", "Content-Type: text/plain;charset=utf-8"
                );
            }
        }

        private void Browser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            KeyDown?.Invoke(sender, e);
        }

        private void Browser_DocumentCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e) {
            PageFinished?.Invoke(sender, e);
        }

        public override void Dispose() {
            ((IDisposable)browser).Dispose();
        }

        public override Control GetControl() {
            return browser;
        }

        public override async Task<object> InvokeScript(string script) {
            var resultString = await browser.ExecuteScriptAsync(script);
            if (resultString == "undefined" || resultString == "null") return null;
            return UnescapeJsString(resultString);
        }

        public override void Navigate(string url) {
            browser.Source = new Uri(url);
        }

        public override void Reload() {
            browser.Reload();
        }

        public override void ShowDevTools() {
            browser.CoreWebView2.OpenDevToolsWindow();
        }
    }
}
