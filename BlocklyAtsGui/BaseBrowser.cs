using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BlocklyATS {

    public abstract class BaseBrowser : IDisposable {
        public abstract Control GetControl();
        public abstract void Reload();
        public abstract void Navigate(string url);
        public abstract event EventHandler PageFinished;
        public abstract event PreviewKeyDownEventHandler KeyDown;
        public abstract object InvokeScript(string script);
        public abstract void Dispose();

        public virtual void BindTo(Control parent) {
            var browser = GetControl();
            browser.Dock = DockStyle.Fill;
            browser.Size = parent.ClientSize;
            parent.Controls.Add(browser);
            browser.BringToFront();
        }

        public static BaseBrowser AcquireInstance(string url = "about:blank") {
#if MONO
            return new WinformBrowser(url);
#else
            var CEFAvailable = File.Exists(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),
                "x86", "CefSharp.dll"));
            if (CEFAvailable) {
                return new CefBrowser(url);
            } else {
                return new WinformBrowser(url);
            }
#endif
        }

        public static string EscapeJsString(string s) {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s) {
                switch (c) {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127) {
                            sb.AppendFormat("\\u{0:X04}", i);
                        } else {
                            sb.Append(c);
                        }
                        break;
                }
            }
            return sb.ToString();
        }

        public void BkyResetWorkspace() {
            InvokeScript("batsWkspReset();");
        }

        public XElement BkySaveWorkspace() {
            var element = XElement.Parse(InvokeScript("batsWkspSave();").ToString());
            element.RemoveAttributes();
            return element;
        }

        public void BkyLoadWorkspace(XElement bkyxml) {
            var arg = EscapeJsString(bkyxml.ToString(SaveOptions.DisableFormatting));
            InvokeScript(string.Format("batsWkspLoad('{0}');", arg));
        }

        public string BkyExportLua() {
            return InvokeScript("batsWkspExportLua();").ToString();
        }

        public string BkyExportCSharp() {
            return InvokeScript("batsWkspExportCSharp();").ToString();
        }
    }
}
