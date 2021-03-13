using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace BlocklyAts {
    partial class WebView2Browser : BaseBrowser {
        public override event EventHandler PageFinished;
        public override event PreviewKeyDownEventHandler KeyDown;

        private CoreWebView2Environment environment;
        private WebView2 browser;

        public WebView2Browser(string url = "about:blank") {
            // Use an alternative path for WebView2 Loader, for a better appearance
            // Might remove it if it causes troubles
            TryLoadWebview2Loader();

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
                //browser.CoreWebView2.Settings.AreDevToolsEnabled = false;
                browser.CoreWebView2.Settings.IsStatusBarEnabled = false;
                browser.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            };
            browser.EnsureCoreWebView2Async(environment);
            Navigate(url);
        }

        private void CoreWebView2_WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e) {
            Uri parsedUri = new Uri(e.Request.Uri);
            string path;
            if (parsedUri.Scheme == "file") {
                path = parsedUri.LocalPath;
            } else if (parsedUri.Scheme == "http" && parsedUri.Host == "make.urlparser.happy") {
                path = parsedUri.LocalPath.TrimStart('/', '\\');
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
                string errorMessageTemplate = @"
<h2>Error: 404 Not Found</h2>
<p>Please inform developer with these information: <a href='mailto:zbx1425@outlook.com'>zbx1425@outlook.com</a></p>
<hr/>
<table>
    <tr><th>Missing Path</th><td>{0}</td></tr>
    <tr><th>URL String</th><td>{1}</td></tr>
    {2}
</table>
                ";
                var urlParsed = new Uri(e.Request.Uri);
                var errorMessage = string.Format(errorMessageTemplate, path, e.Request.Uri,
                    string.Join("\n", urlParsed.GetType().GetProperties()
                    .Select(t => string.Format("<tr><th>{0}</th><td>{1}</td></tr>", t.Name, t.GetValue(urlParsed))))
                );
                e.Response = environment.CreateWebResourceResponse(
                    new MemoryStream(Encoding.UTF8.GetBytes(errorMessage)),
                    404, "Not Found", "Content-Type: text/html;charset=utf-8"
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
            if (resultString == null) return null;
            if (resultString == "undefined" || resultString == "null") return null;
            return UnescapeJsString(resultString);
        }

        public override void Navigate(string url) {
            if (url[1] == ':' && url[2] == '\\') {
                // URLParser says file urls cannot have query param. Make them happy.
                browser.Source = new Uri("http://make.urlparser.happy/" + url);
            } else {
                browser.Source = new Uri(url);
            }
        }

        public override void Reload() {
            browser.Reload();
        }

        public override void ShowDevTools() {
            browser.CoreWebView2.OpenDevToolsWindow();
        }

    }
}
