using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using FXTVGame.Backend.Services;

TcpListener tcpListener = new TcpListener(IPAddress.Any, 12345);
PacketService packetService = new PacketService();
tcpListener.Start();

while (true)
{
    var tcpClient = await tcpListener.AcceptTcpClientAsync();
    Console.WriteLine("SOMEONE CONNECTED");
    _ = HandleClientAsync(tcpClient);    
}

async Task HandleClientAsync(TcpClient tcpClient)
{
    using (tcpClient)
    {
        NetworkStream netStream = tcpClient.GetStream();
        byte[] headerBuffer = new byte[6];

        try
        {

            while (tcpClient.Connected)
            {

                int headerBytesRead = await netStream.ReadAsync(headerBuffer, 0, headerBuffer.Length);

                if (headerBytesRead == 0) break;
                if (headerBytesRead < 6) continue;

                int totalLength = BitConverter.ToInt32(headerBuffer, 0);
                int opCode = BitConverter.ToInt16(headerBuffer, 4);

                int payloadLength = totalLength - 6;
                byte[] payloadBuffer = new byte[payloadLength];
                int payloadByteRead = await netStream.ReadAsync(payloadBuffer, 0, payloadLength);


                if (opCode == 1001)
                {
                    await HandleLoginAsync(payloadBuffer, netStream);
                }
                if (opCode == 1002)
                {
                    await HandleRegisterAsync(payloadBuffer, netStream);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Client xay ra loi: {ex.Message}");
        }

        Console.WriteLine("CLIENT DISCONNECTED");
    }
}

async Task HandleLoginAsync(byte[] payloadBuffer, NetworkStream network)
{
    int currentOffset = 0;
    byte usernameLength = payloadBuffer[currentOffset];
    currentOffset++;

    string username = Encoding.UTF8.GetString(payloadBuffer, currentOffset, usernameLength);
    currentOffset += usernameLength;


    byte passLength = payloadBuffer[currentOffset];
    currentOffset++;

    string password = Encoding.UTF8.GetString(payloadBuffer, currentOffset, passLength);
    currentOffset += passLength;

    Console.WriteLine($"Backend phat hien yeu cau dang nhap:\nUser = {username}, Pass = {password}");

    var databaseService = new DatabaseService();
    var databaseResult = await databaseService.CheckIfUserExistsAndGetIdAsync(username);
    byte[] packet;

    if (!databaseResult.SearchResult)
    {
        packet = packetService.CreateLoginResultPacket(false, username);
        Console.WriteLine("Search failed");
    }
    else
    {
        if ( !await databaseService.CheckPasswordAsync(databaseResult.UserId, password))
        {
            packet = packetService.CreateLoginResultPacket(false, username);
            Console.WriteLine("Check pass wrong");
        }   
        else
        {
            packet = packetService.CreateLoginResultPacket(true, username, databaseResult.UserId);
            Console.WriteLine("Login Succeed");
        }
    }
    await network.WriteAsync(packet, 0, packet.Length);

}
async Task HandleRegisterAsync(byte[] payloadBuffer, NetworkStream network)
{
    int currentOffset = 0;
    byte usernameLength = payloadBuffer[currentOffset];
    currentOffset++;

    string username = Encoding.UTF8.GetString(payloadBuffer, currentOffset, usernameLength);
    currentOffset += usernameLength;


    byte passLength = payloadBuffer[currentOffset];
    currentOffset++;

    string password = Encoding.UTF8.GetString(payloadBuffer, currentOffset, passLength);
    currentOffset += passLength;

    Console.WriteLine($"Backend phat hien yeu cau dang ki:\nUser = {username}, Pass = {password}");

    var databaseService = new DatabaseService();
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

