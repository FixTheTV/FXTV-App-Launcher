using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;

TcpListener tcpListener = new TcpListener(IPAddress.Any, 12345);
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
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Client xay ra loi: {ex.Message}");
        }

        Console.WriteLine("CLIENT DISCONNECTED");
        Console.Out.Flush();
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

    int returnPacketLength = 7;
    byte[] packet = new byte[returnPacketLength];

    Array.Copy(BitConverter.GetBytes(returnPacketLength), 0, packet, 0, 4);
    Array.Copy(BitConverter.GetBytes((short)2001), 0, packet, 4, 2);

    int currentOffset2 = 6;

    packet[currentOffset2] = 1;
    currentOffset2++;

    await network.WriteAsync(packet, 0, returnPacketLength);

}

