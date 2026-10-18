using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using FXTVGame.Launcher.Models;

namespace FXTVGame.Launcher.Services
{
    internal class HTTPService
    {
        private readonly HttpListener _httpListener;
        private readonly int LOCALPORT;

        public Func<GameResultModel, Task>? MatchFinishedAsync { get; set; }

        public HTTPService()
        {
            LOCALPORT = GetFreeTcpPort();
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add($"http://127.0.0.1:{LOCALPORT}/");
            _httpListener.Start();

            _ = Task.Run(ListenForGodotSignals);
        }

        private async Task ListenForGodotSignals()
        {
            while (_httpListener.IsListening)
            {
                HttpListenerContext? context = null;
                try
                {
                    context = await _httpListener.GetContextAsync();
                    HttpListenerRequest request = context.Request;
                    if (request.HttpMethod != "POST" || request.Url?.AbsolutePath != "/match-end")
                    {
                        await WriteResponseAsync(context.Response, 404, "Not found.");
                        continue;
                    }

                    string jsonResult;
                    using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        jsonResult = await reader.ReadToEndAsync();
                    }

                    GameResultModel? result;
                    try
                    {
                        result = JsonSerializer.Deserialize<GameResultModel>(jsonResult);
                    }
                    catch (JsonException)
                    {
                        await WriteResponseAsync(context.Response, 400, "Invalid JSON.");
                        continue;
                    }

                    if (result == null ||
                        result.Status != "completed" ||
                        result.GameId <= 0 ||
                        result.Result is not ("win" or "lose" or "draw"))
                    {
                        await WriteResponseAsync(context.Response, 400, "Invalid game result.");
                        continue;
                    }

                    Func<GameResultModel, Task>? handler = MatchFinishedAsync;
                    if (handler == null)
                    {
                        await WriteResponseAsync(context.Response, 503, "Game result handler is unavailable.");
                        continue;
                    }

                    await handler(result);
                    await WriteResponseAsync(context.Response, 200, "OK");
                    _httpListener.Stop();
                    return;
                }
                catch (HttpListenerException) when (!_httpListener.IsListening)
                {
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to process Godot result callback: {ex}");
                    if (context != null)
                    {
                        try
                        {
                            await WriteResponseAsync(context.Response, 500, "Could not process game result.");
                        }
                        catch
                        {
                            // The client may already have closed the request.
                        }
                    }
                }
            }
        }

        private static async Task WriteResponseAsync(HttpListenerResponse response, int statusCode, string body)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(body);
            response.StatusCode = statusCode;
            response.ContentType = "text/plain; charset=utf-8";
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.Close();
        }

        public int getLocalPort()
        {
            return LOCALPORT;
        }

        private static int GetFreeTcpPort()
        {
            using var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            return ((IPEndPoint)listener.LocalEndpoint).Port;
        }
    }
}
