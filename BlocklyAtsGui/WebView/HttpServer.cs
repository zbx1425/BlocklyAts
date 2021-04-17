using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.NetworkInformation;

namespace BlocklyATS {
    class HttpServer {

        public string ListenUrl = null;

        private HttpListener listener;
        private Task handlerTask;
        private CancellationTokenSource cancelSource;

        public class InteropReceivedEventArgs : EventArgs {
            public string Endpoint { get; set; }
            public Stream InputStream { get; set; }
        }

        public Func<string, string, string> InteropReceived;

        public bool Start() {
            // No need to check for avalibility. You can't install .net 4.6 on XP after all
            var port = GetAvailablePort(33033);
            if (port == 0) return false;

            ListenUrl = "http://127.0.0.1:" + port + "/";
            listener = new HttpListener();
            listener.Prefixes.Add(ListenUrl);
            listener.Start();

            cancelSource = new CancellationTokenSource();
            handlerTask = Task.Run(() => HandleIncomingConnections(cancelSource.Token));
            return true;
        }

        public void Stop() {
            if (listener != null) {
                if (cancelSource != null) cancelSource.Cancel();
                listener.Close();
            }
        }

        private async Task HandleIncomingConnections(CancellationToken cancelToken) {
            while (!cancelToken.IsCancellationRequested) {
                HttpListenerContext ctx = await listener.GetContextAsync();
                if (ctx == null) continue;
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;
                
                resp.AddHeader("X-Content-Type-Options", "nosniff");
                
                var requestPath = req.Url.AbsolutePath.ToString();
                if (requestPath == "/") requestPath = "/index.html"; // Not a nice implemention
                if (requestPath.Contains("..")) {
                    resp.StatusCode = 403;
                    resp.Close();
                    continue;
                } else if (requestPath.StartsWith("/interop/")) {
                    var endpoint = requestPath.Substring("/interop/".Length).ToLowerInvariant();
                    string requestBody;
                    using (var sr = new StreamReader(req.InputStream, Encoding.UTF8))
                        requestBody = sr.ReadToEnd();
                    var responseBody = InteropReceived?.Invoke(endpoint, requestBody);
                    if (responseBody != null) {
                        byte[] data = Encoding.UTF8.GetBytes(responseBody);
                        resp.ContentType = "application/xml; charset=utf-8";
                        resp.ContentEncoding = Encoding.UTF8;
                        resp.ContentLength64 = data.LongLength;
                        await resp.OutputStream.WriteAsync(data, 0, data.Length);
                    }
                    resp.AddHeader("Cache-Control", "no-cache, no-store, max-age=0");
                    resp.Close();
                    continue;
                }
#if DEBUG
                var requestFile = Path.GetFullPath("../www" + requestPath);
                if (!File.Exists(requestFile)) requestFile = Path.GetFullPath("www" + requestPath);
#else
                var requestFile = Path.GetFullPath("www" + requestPath);
#endif
                if (File.Exists(requestFile)) {
                    byte[] data = File.ReadAllBytes(requestFile);
                    switch (Path.GetExtension(requestFile).ToLowerInvariant()) {
                        case ".htm":
                        case ".html":
                            resp.ContentType = "text/html";
                            resp.ContentEncoding = Encoding.UTF8;
                            resp.AddHeader("X-UA-Compatible", "IE=edge");
                            break;
                        case ".js":
                            resp.ContentType = "application/javascript";
                            resp.ContentEncoding = Encoding.UTF8;
                            break;
                        case ".css":
                            resp.ContentType = "text/css";
                            resp.ContentEncoding = Encoding.UTF8;
                            break;
                        case ".xml":
                            resp.ContentType = "application/xml";
                            resp.ContentEncoding = Encoding.UTF8;
                            break;
                        default:
                            resp.ContentType = "application/octet-stream";
                            break;
                    }
                    resp.ContentType += "; charset=utf-8";
                    resp.ContentLength64 = data.LongLength;
                    await resp.OutputStream.WriteAsync(data, 0, data.Length);
                    resp.Close();
                } else {
                    resp.StatusCode = 404;
                    string errorMessageTemplate = @"
<h2>Error: 404 Not Found</h2>
<p>Please inform developer with these information.
<hr/>
<table>
    <tr><th>Missing Path</th><td>{0}</td></tr>
    <tr><th>URL String</th><td>{1}</td></tr>
</table>
                ";
                    var errorMessage = string.Format(errorMessageTemplate, requestFile, requestPath);
                    byte[] data = Encoding.UTF8.GetBytes(errorMessage);
                    resp.ContentType = "text/html; charset=utf-8";
                    resp.ContentEncoding = Encoding.UTF8;
                    resp.ContentLength64 = data.LongLength;
                    await resp.OutputStream.WriteAsync(data, 0, data.Length);
                    resp.Close();
                }
            }
        }

        private static int GetAvailablePort(int startingPort) {
            var portArray = new List<int>();

            var properties = IPGlobalProperties.GetIPGlobalProperties();

            // Ignore active connections
            var connections = properties.GetActiveTcpConnections();
            portArray.AddRange(from n in connections
                               where n.LocalEndPoint.Port >= startingPort
                               select n.LocalEndPoint.Port);

            // Ignore active tcp listners
            var endPoints = properties.GetActiveTcpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               select n.Port);

            // Ignore active UDP listeners
            endPoints = properties.GetActiveUdpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               select n.Port);

            portArray.Sort();

            for (var i = startingPort; i < UInt16.MaxValue; i++)
                if (!portArray.Contains(i))
                    return i;

            return 0;
        }
    }
}
