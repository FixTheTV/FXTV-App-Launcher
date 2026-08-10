using FXTVGame.Backend.Network;
using FXTVGame.Backend.Services;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

ConcurrentDictionary<long, ClientPeer> peers = new ConcurrentDictionary<long, ClientPeer>();
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

        if (peer.UserId > 0)
        {
            peers.TryRemove(peer.UserId, out _);
        }
        string clientPort = ((IPEndPoint)peer.Socket.Client.RemoteEndPoint).Port.ToString();

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