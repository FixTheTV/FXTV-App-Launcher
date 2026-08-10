using FXTVGame.Launcher.Models.Auth;
using FXTVGame.Launcher.Models.Register;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FXTVGame.Launcher.Services
{
    internal class NetworkService
    {
        private readonly PacketService packetService = new PacketService();
        public readonly TcpClient tcpClient;

        public NetworkService()
        {
            tcpClient = new TcpClient();
        }

        public bool IsConnected => tcpClient != null && tcpClient.Connected;

        public async Task ConnectAsync(string ip, int port)
        {
            var convertedIP = IPAddress.Parse(ip);
            try { await tcpClient.ConnectAsync(convertedIP, port); } catch { }
            ;
        }

        public async Task SendLoginPacket(string username, string password)
        {
            var stream = tcpClient.GetStream();
            byte[] packet = packetService.CreateLoginRequestPacket(username, password);
            await stream.WriteAsync(packet, 0, packet.Length);
        }

        public async Task SendRegisterPacket(string username, string password)
        {
            var stream = tcpClient.GetStream();
            byte[] packet = packetService.CreateRegisterRequestPacket(username, password);
            await stream.WriteAsync(packet, 0, packet.Length);
        }

        public async Task<AuthResult> RecieveLoginResultPacket()
        {
            var stream = tcpClient.GetStream();

            byte[] headerBuffer = new byte[6];
            await ReadExactAsync(stream, headerBuffer, 6);

            int length = BitConverter.ToInt32(headerBuffer, 0);
            int opCode = BitConverter.ToInt16(headerBuffer, 4);

            int payloadLength = length - 6;
            byte[] payloadBuffer = new byte[payloadLength];
            await ReadExactAsync(stream, payloadBuffer, payloadLength);

            return HandleLoginResult(payloadBuffer);
        }

        public async Task<RegisterResult> RecieveRegisterResultPacket()
        {
            var stream = tcpClient.GetStream();

            byte[] headerBuffer = new byte[6];
            await ReadExactAsync(stream, headerBuffer, 6);

            int length = BitConverter.ToInt32(headerBuffer, 0);
            int opCode = BitConverter.ToInt16(headerBuffer, 4);

            int payloadLength = length - 6;
            byte[] payloadBuffer = new byte[payloadLength];
            await ReadExactAsync(stream, payloadBuffer, payloadLength);

            return HandleRegisterResult(payloadBuffer);
        }

        private RegisterResult HandleRegisterResult(byte[] payloadBuffer)
        {
            int currentOffset = 0;

            if (payloadBuffer[currentOffset] == 1)
            {
                currentOffset++;
                int usernameLength = payloadBuffer[currentOffset];
                currentOffset++;

                string username = Encoding.UTF8.GetString(payloadBuffer, currentOffset, usernameLength);
                currentOffset += usernameLength;

                return new RegisterResult { Success = true, Message = "Sign in successfully"};
                
            }
            else
            {
                return new RegisterResult { Success = false, Message = "Username already taken"};
            }
        }

        private AuthResult HandleLoginResult(byte[] payloadBuffer)
        {
            int currentOffset = 0;

            if (payloadBuffer[currentOffset] == 1) 
            {
                currentOffset++;
                int usernameLength = payloadBuffer[currentOffset];
                currentOffset++;

                string username = Encoding.UTF8.GetString(payloadBuffer, currentOffset, usernameLength);
                currentOffset += usernameLength;

                Int64 userID = BitConverter.ToInt64(payloadBuffer, currentOffset);

                return new AuthResult { Success = true, Message = "Logged in successfully", Username = username, UserId = userID };
            }
            else 
            {
                return new AuthResult { Success = false, Message = "Incorrect username or password.", Username = "", UserId = 0 };
            }
        }


        private async Task ReadExactAsync(NetworkStream stream, byte[] buffer, int bytesToRead)
        {
            int totalBytesRead = 0;
            while (totalBytesRead < bytesToRead)
            {
                int bytesRead = await stream.ReadAsync(buffer, totalBytesRead, bytesToRead - totalBytesRead);
                if (bytesRead == 0) throw new EndOfStreamException("Kết nối mạng bị đóng bất ngờ!");
                totalBytesRead += bytesRead;
            }
        }

        public async Task DisconnectAsync()
        {
            tcpClient.Close();
        }
    }
}