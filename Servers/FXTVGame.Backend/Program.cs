using FXTVGame.Backend.Network;
using FXTVGame.Backend.Services;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

ConcurrentDictionary<long, ClientPeer> peers = new ConcurrentDictionary<long, ClientPeer>();
ConcurrentDictionary<int, Lobby> lobbies = new ConcurrentDictionary<int, Lobby>();

TcpListener tcpListener = new TcpListener(IPAddress.Any, 12345);
tcpListener.Start();

PacketService packetService = new PacketService();
DatabaseService databaseService = new DatabaseService();

Console.WriteLine("Server Started on Port 12345...");

while (true)
{
    try
    {
        TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

        ClientPeer peer = new ClientPeer(tcpClient);
        Console.WriteLine($"[Connect] Client connected from {tcpClient.Client.RemoteEndPoint}");

        _ = Task.Run(() => HandleClientAsync(peer));
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Accept Error] {ex.Message}");
    }
}

async Task HandleClientAsync(ClientPeer peer)
{
    using (peer.Socket)
    {
        NetworkStream netStream = peer.Socket.GetStream();
        byte[] headerBuffer = new byte[6];

        try
        {
            while (peer.Socket.Connected)
            {
                await ReadExactAsync(netStream, headerBuffer, 6);

                int totalLength = BitConverter.ToInt32(headerBuffer, 0);
                ushort opCode = BitConverter.ToUInt16(headerBuffer, 4);

                int payloadLength = totalLength - 6;
                byte[] payloadBuffer = new byte[payloadLength];

                await ReadExactAsync(netStream, payloadBuffer, payloadLength);

                switch (opCode)
                {
                    case Opcodes.C2S_Login:
                        if (peer.State == ClientState.Connected)
                        {
                            await HandleLoginAsync(peer, payloadBuffer, netStream);
                        }
                        break;

                    case Opcodes.C2S_Register:
                        if (peer.State == ClientState.Connected)
                        {
                            await HandleRegisterAsync(peer, payloadBuffer, netStream);
                        }
                        break;

                    case Opcodes.C2S_Logout:
                        if (peer.State != ClientState.Connected)
                        {
                            await HandleLogoutAsync(peer, netStream);
                        }
                        break;

                    case Opcodes.C2S_JoinLobby:
                        if (peer.State == ClientState.Authenticated)
                        {
                            await HandleJoinLobbyAsync(peer, payloadBuffer, netStream);
                        }
                        break;

                    case Opcodes.C2S_LobbyChat:
                        if (peer.State == ClientState.InLobby)
                        {
                            await HandleLobbyChatAsync(peer, payloadBuffer);
                        }
                        break;

                    case Opcodes.C2S_LobbyReady:
                        if (peer.State == ClientState.InLobby)
                        {
                            await HandleLobbyReadyAsync(peer, payloadBuffer);
                        }
                        break;

                    case Opcodes.C2S_LeaveLobby:
                        if (peer.State == ClientState.InLobby)
                        {
                            await HandleLeaveLobbyAsync(peer, netStream);
                            
                        }
                        break;

                    default:
                        Console.WriteLine($"[Warning] Unknown Opcode: {opCode}");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Client Loop Error] {ex.Message}");
        }

        await RemovePeerFromLobby(peer);
        peers.TryRemove(peer.UserId, out _);

        string clientPort = peer.Socket.Client.RemoteEndPoint is IPEndPoint remoteEndPoint
            ? remoteEndPoint.Port.ToString()
            : "unknown";

        Console.WriteLine($"[Disconnect] User ID: {peer.UserId} (Session Port: {clientPort}) disconnected.");
    }
}


async Task HandleLoginAsync(ClientPeer peer, byte[] payloadBuffer, NetworkStream network)
{
    int currentOffset = 0;
    byte usernameLength = payloadBuffer[currentOffset++];
    string username = Encoding.UTF8.GetString(payloadBuffer, currentOffset, usernameLength);
    currentOffset += usernameLength;

    byte passLength = payloadBuffer[currentOffset++];
    string password = Encoding.UTF8.GetString(payloadBuffer, currentOffset, passLength);

    Console.WriteLine($"[Login Request] User = {username}");

    var databaseResult = await databaseService.CheckIfUserExistsAndGetIdAsync(username);
    byte[] packet;

    if (!databaseResult.SearchResult || !await databaseService.CheckPasswordAsync(databaseResult.UserId, password))
    {
        packet = packetService.CreateLoginResultPacket(false, username);
        Console.WriteLine("Login Failed");
    }
    else
    {
        
        peer.UserId = databaseResult.UserId;
        peer.Username = username;
        peer.State = ClientState.Authenticated; 
        
        packet = packetService.CreateLoginResultPacket(true, username, peer.UserId);
        peers.TryAdd(peer.UserId, peer);
        Console.WriteLine($"Login Succeed! Generated Token for User {peer.UserId}");
    }

    await network.WriteAsync(packet, 0, packet.Length);
}

async Task HandleRegisterAsync(ClientPeer peer, byte[] payloadBuffer, NetworkStream network)
{
    int currentOffset = 0;
    byte usernameLength = payloadBuffer[currentOffset++];
    string username = Encoding.UTF8.GetString(payloadBuffer, currentOffset, usernameLength);
    currentOffset += usernameLength;

    byte passLength = payloadBuffer[currentOffset++];
    string password = Encoding.UTF8.GetString(payloadBuffer, currentOffset, passLength);

    var databaseResult = await databaseService.CheckIfUserExistsAndGetIdAsync(username);
    byte[] packet;

    if (databaseResult.SearchResult)
    {
        packet = packetService.CreateRegisterResultPacket(false);
    }
    else
    {
        await databaseService.AddUserAsync(username, password);
        packet = packetService.CreateRegisterResultPacket(true, username);
    }

    await network.WriteAsync(packet, 0, packet.Length);
}

async Task HandleLogoutAsync(ClientPeer peer, NetworkStream network)
{
    await RemovePeerFromLobby(peer);
    peers.TryRemove(peer.UserId, out _);

    peer.UserId = 0;
    peer.Username = string.Empty;
    peer.State = ClientState.Connected;

    byte[] packet = packetService.CreateLogoutResultPacket(true);
    await network.WriteAsync(packet, 0, packet.Length);
}

async Task HandleJoinLobbyAsync(ClientPeer peer, byte[] payloadBuffer, NetworkStream network)
{
    int lobbyId = BitConverter.ToInt32(payloadBuffer, 0);

    await RemovePeerFromLobby(peer);

    Lobby lobby = lobbies.GetOrAdd(lobbyId, id => new Lobby(id));

    byte[] packet;
    bool joinedLobby;
    int playerCount;

    lock (lobby.SyncRoot)
    {
        if (lobby.Peers.Count >= lobby.PeerSlots.Length)
        {
            packet = packetService.CreateJoinLobbyResultPacket(false, lobbyId, lobby.Peers.Count);
            playerCount = lobby.Peers.Count;
            joinedLobby = false;
        }
        else
        {
            for (int i = 0; i < lobby.PeerSlots.Length; i++)
            {
                if (lobby.PeerSlots[i] == null)
                {
                    lobby.PeerSlots[i] = peer;
                    break;
                }
            }

            lobby.Peers[peer.UserId] = peer;
            playerCount = lobby.Peers.Count;
            packet = packetService.CreateJoinLobbyResultPacket(true, lobbyId, playerCount);
            joinedLobby = true;
        }
    }

    if (!joinedLobby)
    {
        await network.WriteAsync(packet, 0, packet.Length);
        return;
    }

    peer.LobbyId = lobbyId;
    peer.State = ClientState.InLobby;
    peer.IsReady = false;

    await network.WriteAsync(packet, 0, packet.Length);

    await BroadcastUpdateLobbyCount(lobby, playerCount);
    await BroadcastLobbyTabsAsync(lobby);
    await BroadcastLobbyChatAsync(lobby, "Server", $"{peer.Username} joined lobby {lobbyId}.");
}


async Task BroadcastLobbyTabsAsync(Lobby lobby)
{
    List<(string Name, byte Slot, bool IsReady)> lobbyTabs = new List<(string Name, byte Slot, bool IsReady)>();

    lock (lobby.SyncRoot)
    {
        for (byte i = 0; i < lobby.PeerSlots.Length; i++)
        {
            ClientPeer? slotPeer = lobby.PeerSlots[i];
            lobbyTabs.Add((slotPeer?.Username ?? string.Empty, i, slotPeer?.IsReady ?? false));
        }
    }

    foreach ((string name, byte slot, bool isReady) in lobbyTabs)
    {
        await BroadcastUpdateLobbyTab(lobby, name, slot, isReady);
    }
}

async Task BroadcastUpdateLobbyTab(Lobby lobby, string name, byte slot, bool isReady)
{
    byte[] packet = packetService.CreateUpdateLobbyTabPacket(name, slot, isReady);
    List<long> disconnectedPeers = new List<long>();

    foreach (var peerEntry in lobby.Peers)
    {
        ClientPeer targetPeer = peerEntry.Value;

        try
        {
            NetworkStream stream = targetPeer.Socket.GetStream();
            await stream.WriteAsync(packet, 0, packet.Length);
        }
        catch
        {
            disconnectedPeers.Add(peerEntry.Key);
        }
    }

    foreach (long userId in disconnectedPeers)
    {
        RemovePeerFromLobbyState(lobby, userId);
    }
}

async Task HandleLobbyReadyAsync(ClientPeer peer, byte[] payloadBuffer)
{
    if (peer.LobbyId == null || payloadBuffer.Length < 1)
    {
        return;
    }

    peer.IsReady = payloadBuffer[0] == 1;

    if (lobbies.TryGetValue(peer.LobbyId.Value, out Lobby? lobby))
    {
        await BroadcastLobbyTabsAsync(lobby);
    }
}

async Task HandleLeaveLobbyAsync(ClientPeer peer, NetworkStream netStream)
{
    byte[] packet = packetService.CreatePacketHeader(6, 2104);
    await RemovePeerFromLobby(peer);
    await netStream.WriteAsync(packet);
}
async Task BroadcastUpdateLobbyCount(Lobby lobby, int count)
{
    byte[] packet = packetService.CreateUpdateCountLobbyPacket(count);
    List<long> disconnectedPeers = new List<long>();

    foreach (var peerEntry in lobby.Peers)
    {
        ClientPeer targetPeer = peerEntry.Value;

        try
        {
            NetworkStream stream = targetPeer.Socket.GetStream();
            await stream.WriteAsync(packet, 0, packet.Length);
        }
        catch
        {
            disconnectedPeers.Add(peerEntry.Key);
        }
    }

    foreach (long userId in disconnectedPeers)
    {
        RemovePeerFromLobbyState(lobby, userId);
    }
}

async Task HandleLobbyChatAsync(ClientPeer peer, byte[] payloadBuffer)
{
    if (peer.LobbyId == null)
    {
        return;
    }

    int currentOffset = 0;
    ushort messageLength = BitConverter.ToUInt16(payloadBuffer, currentOffset);
    currentOffset += 2;
    string message = Encoding.UTF8.GetString(payloadBuffer, currentOffset, messageLength);

    if (!lobbies.TryGetValue(peer.LobbyId.Value, out Lobby? lobby))
    {
        return;
    }

    await BroadcastLobbyChatAsync(lobby, peer.Username, message);
}

async Task BroadcastLobbyChatAsync(Lobby lobby, string username, string message)
{
    byte[] packet = packetService.CreateLobbyChatPacket(username, message);
    List<long> disconnectedPeers = new List<long>();

    foreach (var peerEntry in lobby.Peers)
    {
        ClientPeer targetPeer = peerEntry.Value;

        try
        {
            NetworkStream stream = targetPeer.Socket.GetStream();
            await stream.WriteAsync(packet, 0, packet.Length);
        }
        catch
        {
            disconnectedPeers.Add(peerEntry.Key);
        }
    }

    foreach (long userId in disconnectedPeers)
    {
        RemovePeerFromLobbyState(lobby, userId);
    }
}

async Task RemovePeerFromLobby(ClientPeer peer)
{
    if (peer.LobbyId == null)
    {
        return;
    }

    string username = peer.Username;
    int lobbyId = peer.LobbyId.Value;
    Lobby? lobbyToUpdate = null;
    int playerCount = 0;

    if (lobbies.TryGetValue(peer.LobbyId.Value, out Lobby? lobby))
    {
        RemovePeerFromLobbyState(lobby, peer.UserId);
        playerCount = lobby.Peers.Count;

        if (lobby.Peers.IsEmpty)
        {
            lobbies.TryRemove(lobby.Id, out _);
        }
        else
        {
            lobbyToUpdate = lobby;
        }
    }

    peer.LobbyId = null;

    if (peer.UserId > 0)
    {
        peer.State = ClientState.Authenticated;
    }

    if (lobbyToUpdate != null)
    {
        await BroadcastUpdateLobbyCount(lobbyToUpdate, playerCount);
        await BroadcastLobbyTabsAsync(lobbyToUpdate);
        await BroadcastLobbyChatAsync(lobbyToUpdate, "Server", $"{username} left lobby {lobbyId}.");
    }
}

void RemovePeerFromLobbyState(Lobby lobby, long userId)
{
    lock (lobby.SyncRoot)
    {
        lobby.Peers.TryRemove(userId, out _);

        for (int i = 0; i < lobby.PeerSlots.Length; i++)
        {
            if (lobby.PeerSlots[i]?.UserId == userId)
            {
                lobby.PeerSlots[i] = null;
            }
        }
    }
}

async Task ReadExactAsync(NetworkStream stream, byte[] buffer, int bytesToRead)
{
    int totalBytesRead = 0;
    while (totalBytesRead < bytesToRead)
    {
        int bytesRead = await stream.ReadAsync(buffer, totalBytesRead, bytesToRead - totalBytesRead);
        if (bytesRead == 0) throw new EndOfStreamException("Kết nối mạng bị đóng bất ngờ!");
        totalBytesRead += bytesRead;
    }
}
