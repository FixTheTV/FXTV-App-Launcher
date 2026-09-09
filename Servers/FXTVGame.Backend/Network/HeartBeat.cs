using FXTVGame.Backend.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Timers;

namespace FXTVGame.Backend.Network
{
    internal class HeartBeat
    {
        private CancellationTokenSource cts = new CancellationTokenSource();
        private readonly PacketService _packetService;
        public HeartBeat(PacketService packetService)
        {
            _packetService = packetService;
        } 
        public void StartHeartBeatLoop(ConcurrentDictionary<long, ClientPeer> peers)
        {
            Task.Run(async () =>
             { 
                using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

                while (await timer.WaitForNextTickAsync(cts.Token))
                {
                    if (peers.IsEmpty) continue;

                    foreach (var kvp in peers)
                     {
                         long peerID = kvp.Key;
                         ClientPeer clientPeer = kvp.Value;

                         if (!clientPeer.IsAlive)
                         {
                             HandleDisconnection(peerID,peers,clientPeer);
                             continue;
                         }

                         clientPeer.IsAlive = false;

                         _ = SendPingAsync(clientPeer);
                     }
                }
            });
        }

        private void HandleDisconnection(long id, ConcurrentDictionary<long, ClientPeer>peers, ClientPeer clientPeer)
        {
            if (peers.TryRemove(id, out _)){
                clientPeer.Socket?.Close();
                Console.WriteLine($"Client {id} timed out. Disconnected.");
            }
        }

        private async Task SendPingAsync(ClientPeer clientPeer)
        {
            try
            {
                if (clientPeer.Socket is { Connected : true}){
                    var stream = clientPeer.Socket.GetStream();
                    byte[] packet = _packetService.CreatePingPacket();
                    Console.WriteLine($"Sent Ping to User ID: {clientPeer.UserId}");
                    await stream.WriteAsync(packet, 0, packet.Length); 
                }
            }
            catch
            {

            }
        }
    }
}
