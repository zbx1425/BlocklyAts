using BlocklyATS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlocklyAts {

    class ExternalBrowser : BaseBrowser {
#pragma warning disable CS0067
        public override event EventHandler PageFinished;
        public override event PreviewKeyDownEventHandler KeyDown;

        private const long HeartbeatTimeout = 2000;
        private const int JavascriptTimeout = 10000;
        private const long ElapseInterval = 1000;

        private HttpServer server;
        private Label infoLabel;

        private Dictionary<int, long> heartbeat = new Dictionary<int, long>();
        private List<Tuple<int, string, TaskCompletionSource<string>>> jsQueue
            = new List<Tuple<int, string, TaskCompletionSource<string>>>();
        private int jsQueueNextID = 0;
        private readonly System.Threading.Timer lockdownTimer;

        private Random random = new Random();

        public ExternalBrowser(string url = "about:blank") {
            infoLabel = new Label() {
                AutoSize = false,
                Text = "Please wait...",
                Padding = new Padding(20)
            };
            infoLabel.Font = new System.Drawing.Font(System.Drawing.SystemFonts.CaptionFont.FontFamily, 
                20, System.Drawing.GraphicsUnit.Pixel);
            infoLabel.Click += InfoLabel_LinkClicked;

            server = new HttpServer() { InteropReceived = InteropReceived };
            try {
                if (!server.Start()) {
                    SetMessage("Failed to start HTTP server. Make sure you have a port available above 33033!");
                }
            } catch (Exception ex) {
                SetMessage("Failed to start HTTP server!" + Environment.NewLine + ex.ToString());
                server = null;
            }

            lockdownTimer = new System.Threading.Timer(Elapse, null, 0, ElapseInterval);
        }

        public override void Dispose() {
            server.Stop();
            infoLabel.Dispose();
        }

        public override Control GetControl() {
            return infoLabel;
        }

        public override async Task<object> InvokeScript(string script) {
            if (server == null) {
                MessageBox.Show(
                    "Failed to start HTTP server! Please check the error message.",
                    "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Warning
                );
                return null;
            } else if (heartbeat.Count < 1) {
                MessageBox.Show(
                    "Please open '" + TargetUrl + "' with your system browser!",
                    "Browser not connected", MessageBoxButtons.OK, MessageBoxIcon.Warning
                );
                return null;
            } else if (heartbeat.Count > 1) {
                MessageBox.Show(
                    "Too many clients! Only one tab page of one browser may be connected!",
                    "Too many clients", MessageBoxButtons.OK, MessageBoxIcon.Warning
                );
                return null;
            }
            var tcs = new TaskCompletionSource<string>();
            var id = jsQueueNextID;
            jsQueue.Add(new Tuple<int, string, TaskCompletionSource<string>>(id, script, tcs));
            jsQueueNextID++;
            SetMessage("Awaiting remote Javascript execution... #" + id);
            await Task.WhenAny(tcs.Task, Task.Delay(JavascriptTimeout));
            jsQueue.Remove(jsQueue.First(t => t.Item1 == id));
            return tcs.Task.IsCompleted ? tcs.Task.Result : null;
        }

        private string InteropReceived(string endpoint, string requestBody) {
            if (endpoint == "meta") {
                int browserID;
                do { browserID = random.Next(100000, 1000000); } while (heartbeat.ContainsKey(browserID));
                return string.Format(
                    "<xml><version>{0}</version><id>{1}</id></xml>",
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                    browserID
                );
            } else if (endpoint == "heartbeat") {
                if (!int.TryParse(requestBody, out int clientID)) return null;
                heartbeat[clientID] = DateTime.Now.Ticks;
                var firstPending = jsQueue.FirstOrDefault();
                if (firstPending != null) {
                    return string.Format(
                        "<xml><id>{0}</id><script>{1}</script></xml>",
                        firstPending.Item1,
                        firstPending.Item2
                    );
                } else if (heartbeat.Count > 1) {
                    return "<xml><toomany></toomany></xml>";
                } else {
                    return "<xml></xml>";
                }
            } else if (int.TryParse(endpoint, out int jsQueueID)) {
                var queueItem = jsQueue.First(t => t.Item1 == jsQueueID);
                if (queueItem == null) return null;
                queueItem.Item3.TrySetResult(requestBody);
                SetMessage("Finished remote Javascript execution. #" + jsQueueID);
            }
            return null;
        }

        private bool browserReady = false;
        private readonly object timerLockObject = new object();

        private void Elapse(object target) {
            int tid = Thread.CurrentThread.ManagedThreadId;
            bool lockTaken = false;
            try {
                lockTaken = Monitor.TryEnter(timerLockObject);
                if (!lockTaken) return;
                long latestTolerableTick = DateTime.Now.Ticks - TimeSpan.TicksPerMillisecond * HeartbeatTimeout;
                foreach (var kvp in heartbeat.Where(kvp => kvp.Value < latestTolerableTick).ToArray())
                    heartbeat.Remove(kvp.Key);
                if (heartbeat.Count < 1 && TargetUrl != null) {
                    SetMessage("Please open '" + TargetUrl + "' with your system browser.", true);
                    browserReady = false;
                } else if (heartbeat.Count > 1) {
                    SetMessage("Too many clients! Only one tab page of one browser may be connected.");
                    browserReady = false;
                } else {
                    if (!browserReady) SetMessage("Browser connected. #" + heartbeat.First().Key);
                    browserReady = true;
                }
            } finally {
                if (lockTaken) Monitor.Exit(timerLockObject);
            }
        }

        private void SetMessage(string message, bool enableLink = false) {
            Action action = () => {
                if (infoLabel.Text == message) return;

                infoLabel.Text = message;
                //if (enableLink) {
                //    var linkStart = message.IndexOf('\'');
                //    var linkEnd = message.LastIndexOf('\'');
                //    infoLabel.LinkArea = new LinkArea(linkStart + 1, linkEnd - linkStart - 1);
                //} else {
                //    infoLabel.LinkArea = new LinkArea(0, 0);
                //}
                infoLabel.Refresh();
            };
            if (infoLabel.InvokeRequired) {
                infoLabel.BeginInvoke(action);
            } else {
                action.Invoke();
            }
        }

        private void InfoLabel_LinkClicked(object sender, EventArgs e) {
            if (TargetUrl != null) PlatformFunction.CallBrowser(TargetUrl);
        }

        private string TargetUrl {
            get {
                if (string.IsNullOrEmpty(server.ListenUrl)) return null;
                return server.ListenUrl + "?lang=" + I18n.Translate("BlocklyName");
            }
        }

        public override void Navigate(string url) {
            // Not possible on external browser.
        }

        public override void Reload() {
            // Not possible on external browser.
        }

        public override void ShowDevTools() {
            // Not possible on external browser.
        }
    }
}
