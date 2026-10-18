using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FXTVGame.Backend.Network
{
    public class Lobby
    {
        public int Id { get; }
        public ConcurrentDictionary<long, ClientPeer> Peers { get; } = new ConcurrentDictionary<long, ClientPeer>();
        public ClientPeer?[] PeerSlots { get; } = new ClientPeer?[2];
        public bool HasGameStarted { get; set; }
        public HashSet<long> FinishedPeerIds { get; } = new HashSet<long>();
        public object SyncRoot { get; } = new object();

        public Lobby(int id)
        {
            Id = id;
        }
    }
}
