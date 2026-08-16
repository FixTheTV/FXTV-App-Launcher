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
                        if (peer.State == ClientState.Authenticated || peer.State == ClientState.InLobby)
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

        RemovePeerFromLobby(peer);
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
    RemovePeerFromLobby(peer);
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

    RemovePeerFromLobby(peer);

    Lobby lobby = lobbies.GetOrAdd(lobbyId, id => new Lobby(id));
    lobby.Peers[peer.UserId] = peer;

    peer.LobbyId = lobbyId;
    peer.State = ClientState.InLobby;

    byte[] packet = packetService.CreateJoinLobbyResultPacket(true, lobbyId, lobby.Peers.Count);
    await network.WriteAsync(packet, 0, packet.Length);

    await BroadcastLobbyChatAsync(lobby, "Server", $"{peer.Username} joined lobby {lobbyId}.");
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
        lobby.Peers.TryRemove(userId, out _);
    }
}

void RemovePeerFromLobby(ClientPeer peer)
{
    if (peer.LobbyId == null)
    {
        return;
    }

    if (lobbies.TryGetValue(peer.LobbyId.Value, out Lobby? lobby))
    {
        lobby.Peers.TryRemove(peer.UserId, out _);

        if (lobby.Peers.IsEmpty)
        {
            lobbies.TryRemove(lobby.Id, out _);
        }
    }

    peer.LobbyId = null;

    if (peer.UserId > 0)
    {
        peer.State = ClientState.Authenticated;
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
