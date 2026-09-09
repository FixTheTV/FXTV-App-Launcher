using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace FXTVGame.Backend.Network
{
    public enum ClientState
    {
        Connected,
        Authenticated,
        InLobby,
        InGame
    }
    public class ClientPeer
    {
        public TcpClient Socket { get; private set; }
        public ClientState State { get; set; } = ClientState.Connected;
        public long UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int? LobbyId { get; set; }
        public bool IsReady { get; set; }

        public bool IsAlive { get; set; } = true;

        public ClientPeer(TcpClient socket)
        {
            Socket = socket;
        }
    }
    public class ServerData
    {
        public ConcurrentDictionary<long, ClientPeer> peers;
        public ConcurrentDictionary<int, Lobby> lobbies;

        public ServerData()
        {
            peers = new ConcurrentDictionary<long, ClientPeer>();
            lobbies = new ConcurrentDictionary<int, Lobby>();
        }
    }
}
