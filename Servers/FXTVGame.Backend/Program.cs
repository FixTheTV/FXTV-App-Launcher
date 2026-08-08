using System.Net.Sockets;
using System.Net;

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
        byte[] buffer = new byte[1024];

        int bytesRead = await netStream.ReadAsync(buffer, 0, buffer.Length);

        Console.WriteLine(string.Join(" ",buffer.Take(bytesRead)));
    }


}

