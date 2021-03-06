using BlocklyAts.Host;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BlocklyAts.WebView {

    public abstract class BaseBrowser : IDisposable {

        public class InteropReceivedEventArgs : EventArgs {
            public string Message { get; set; }
            public InteropReceivedEventArgs(string msg) : base() { Message = msg; }
        }

        public abstract Control GetControl();
        public abstract void Reload();
        public abstract void Navigate(string url);
        public abstract event EventHandler PageFinished;
        public abstract event PreviewKeyDownEventHandler KeyDown;
        public abstract event EventHandler<InteropReceivedEventArgs> InteropReceived;
        public abstract Task<object> InvokeScript(string script);
        public abstract void ShowDevTools();
        public abstract void Dispose();

        public virtual void BindTo(Control parent) {
            var browser = GetControl();
            browser.Dock = DockStyle.Fill;
            browser.Size = parent.ClientSize;
            parent.Controls.Add(browser);
            browser.BringToFront();
            browser.Focus();
        }

        public static BaseBrowser AcquireInstance(string url = "about:blank") {
            if (PreferenceManager.Current.PreferExternalBrowser) {
                return new ExternalBrowser(url);
            } else if (!PlatformFunction.IsMono) {
                const string EdgeKeyName = @"SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}";
                var WebView2Available = Registry.LocalMachine.OpenSubKey(EdgeKeyName)?.GetValue("pv", null) != null;
                if (WebView2Available) {
                    return new WebView2Browser(url);
                } else {
                    return new WinformBrowser(url);
                }
            } else {
                return new ExternalBrowser(url);
            }
        }

        private static JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

        public static string EscapeJsString(string s) {
            return jsSerializer.Serialize(s);
        }

        public static string UnescapeJsString(string s) {
            return (string)jsSerializer.Deserialize(s, typeof(string));
        }

        public async Task BkyResetWorkspace() {
            await InvokeScript("batsWkspReset();");
        }

        public async Task<XElement> BkySaveWorkspace() {
            var workspaceString = (await InvokeScript("batsWkspSave();"))?.ToString();
            if (workspaceString == null) return null;
            var element = XElement.Parse(workspaceString);
            element.RemoveAttributes();
            return element;
        }

        public async Task BkyLoadWorkspace(XElement bkyxml) {
            var arg = EscapeJsString(bkyxml.ToString(SaveOptions.DisableFormatting));
            await InvokeScript(string.Format("batsWkspLoad({0});", arg));
        }

        public async Task BkyLoadInitWorkspace(XElement bkyxml) {
            var arg = EscapeJsString(bkyxml.ToString(SaveOptions.DisableFormatting));
            await InvokeScript(string.Format(
                "if (workspace == null) window.onBlocklyLoad = function() {{batsWkspLoad({0});}}; else batsWkspLoad({0});", 
                arg
            ));
        }
        
        public async Task<string> BkyExportCSharp() {
            return (await InvokeScript("batsWkspExportCSharp();"))?.ToString();
        }

        public async Task BkyInformWorkspaceSaved() {
            await InvokeScript("batsOnWorkspaceSaved();");
        }
    }
}
