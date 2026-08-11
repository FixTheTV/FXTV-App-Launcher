using System.Collections.Concurrent;

namespace FXTVGame.Backend.Network
{
    public class    Lobby
    {
        public int Id { get; }
        public ConcurrentDictionary<long, ClientPeer> Peers { get; } = new ConcurrentDictionary<long, ClientPeer>();

        public Lobby(int id)
        {
            Id = id;
        }
    }
}
