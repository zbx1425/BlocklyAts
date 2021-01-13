using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BlocklyATS {
    class HttpServer {

        public const string ListenUrl = "http://localhost:42435/";

        public int MagicNum = new Random().Next(100000000, 1000000000);

        private HttpListener listener;
        private Task handlerTask;
        private CancellationTokenSource cancelSource;

        public class InteropReceivedEventArgs : EventArgs {
            public string Endpoint { get; set; }
            public Stream InputStream { get; set; }
        }

        public event EventHandler<InteropReceivedEventArgs> InteropReceived;

        public void Start() {
            // No need to check for avalibility. You can't install .net 4.6 on XP after all
            listener = new HttpListener();
            listener.Prefixes.Add(ListenUrl);
            listener.Start();

            cancelSource = new CancellationTokenSource();
            handlerTask = Task.Run(() => HandleIncomingConnections(cancelSource.Token));
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

                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();

                if (req.HttpMethod == "GET") {
                    const bool debug = true;
                    var requestPath = req.Url.AbsolutePath.ToString();
                    if (requestPath == "/") requestPath = "/index.html"; // Not a nice implemention
                    if (requestPath.Contains("..")) {
                        resp.StatusCode = 403;
                        resp.Close();
                        continue;
                    } else if (requestPath == "/interop/svrmeta") {
                        string metadata = string.Format(
                            "<xml><version>{0}</version><magicnum>{1}</magicnum></xml>",
                            System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                            this.MagicNum
                        );
                        byte[] data = Encoding.UTF8.GetBytes(metadata);
                        resp.ContentType = "application/xml";
                        resp.ContentEncoding = Encoding.UTF8;
                        resp.ContentLength64 = data.LongLength;
                        await resp.OutputStream.WriteAsync(data, 0, data.Length);
                        resp.Close();
                        continue;
                    } else if (requestPath.StartsWith("/interop/")) {
                        var endpoint = requestPath.Substring("/interop/".Length);
                        InteropReceived(this, new InteropReceivedEventArgs {
                            Endpoint = endpoint.ToLowerInvariant(),
                            InputStream = req.InputStream
                        });
                        resp.AddHeader("Cache-Control", "no-cache, no-store, max-age=0");
                        resp.Close();
                        continue;
                    }
                    var requestFile = Path.GetFullPath((debug ? "../../www" : "www") + requestPath);
                    if (File.Exists(requestFile)) {
                        byte[] data = File.ReadAllBytes(requestFile);
                        switch (Path.GetExtension(requestFile).ToLowerInvariant()) {
                            case ".htm":
                            case ".html":
                                resp.ContentType = "text/html";
                                resp.ContentEncoding = Encoding.UTF8;
                                resp.AddHeader("X-UA-Compatible", "IE-edge");
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
                        resp.ContentLength64 = data.LongLength;
                        await resp.OutputStream.WriteAsync(data, 0, data.Length);
                        resp.Close();
                    } else {
                        resp.StatusCode = 404;
                        resp.Close();
                    }
                } else {
                    resp.StatusCode = 405;
                    resp.Close();
                }
            }
        }
    }
}
