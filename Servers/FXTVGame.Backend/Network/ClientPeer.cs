using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace FXTVGame.Backend.Network
{
    public class ClientPeer
    {
        public TcpClient Socket { get; private set; }
        public ClientState State { get; set; } = ClientState.Connected;
        public long UserId { get; set; }
        public string Username { get; set; }

        public ClientPeer(TcpClient socket)
        {
            Socket = socket;
        }
    }
}
