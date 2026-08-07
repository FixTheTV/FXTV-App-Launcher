using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace FXTVGame.Launcher.Services
{
    internal class NetworkService
    {
        public readonly TcpClient tcpClient;
        public NetworkService()
        {
            tcpClient = new TcpClient();
        }

        public async Task ConnectAsync(IPAddress ip, int port)
        {
            try { await tcpClient.ConnectAsync(ip, port); } catch { };
        }
        public async Task ConnectAsync(string ip, int port)
        {
            var convertedIP = IPAddress.Parse(ip);
            try { await tcpClient.ConnectAsync(convertedIP, port); } catch { };
        }
    }
}
