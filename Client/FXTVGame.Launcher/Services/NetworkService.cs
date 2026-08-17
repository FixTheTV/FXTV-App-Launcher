using FXTVGame.Launcher.Models;
using FXTVGame.Launcher.Models.Auth;
using FXTVGame.Launcher.Models.Register;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FXTVGame.Launcher.Services
{
    internal class NetworkService
    {
        public static NetworkService Shared { get; } = new NetworkService();

        private readonly PacketService packetService = new PacketService();
        private TcpClient tcpClient;
        private bool isReceivingLobbyChat;

        public event Action<LobbyChatMessage>? LobbyChatReceived;
        public event Action<int>? UpdateLobbyCount;

        private NetworkService()
        {
            tcpClient = new TcpClient();
        }

        public bool IsConnected => tcpClient.Connected;

        public async Task ConnectAsync(string ip, int port)
        {
            if (IsConnected)
            {
                return;
            }

            tcpClient?.Dispose();
            tcpClient = new TcpClient();


            if (!IPAddress.TryParse(ip, out var convertedIP))
            {
                throw new ArgumentException("Định dạng IP không hợp lệ", nameof(ip));
            }

            
            await tcpClient.ConnectAsync(convertedIP, port);
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

        public async Task SendLogoutPacket()
        {
            var stream = tcpClient.GetStream();
            byte[] packet = packetService.CreateLogoutRequestPacket();
            await stream.WriteAsync(packet, 0, packet.Length);
        }

        public async Task SendJoinLobbyPacket(int lobbyId)
        {
            var stream = tcpClient.GetStream();
            byte[] packet = packetService.CreateJoinLobbyRequestPacket(lobbyId);
            await stream.WriteAsync(packet, 0, packet.Length);
        }

        public async Task SendLobbyChatPacket(string message)
        {
            var stream = tcpClient.GetStream();
            byte[] packet = packetService.CreateLobbyChatPacket(message);
            await stream.WriteAsync(packet, 0, packet.Length);
        }

        public async Task<AuthResult> RecieveLoginResultPacket()
        {
            PacketData packet = await ReadPacketAsync();
            return HandleLoginResult(packet.Payload);
        }

        public async Task<RegisterResult> RecieveRegisterResultPacket()
        {
            PacketData packet = await ReadPacketAsync();
            return HandleRegisterResult(packet.Payload);
        }

        public async Task<bool> ReceiveLogoutResultPacket()
        {
            PacketData packet = await ReadPacketAsync();
            return packet.Payload.Length > 0 && packet.Payload[0] == 1;
        }

        public async Task<JoinLobbyResult> ReceiveJoinLobbyResultPacket()
        {
            PacketData packet = await ReadPacketAsync();
            return HandleJoinLobbyResult(packet.Payload);
        }

        public void StartLobbyChatReceiveLoop()
        {
            if (isReceivingLobbyChat)
            {
                return;
            }

            isReceivingLobbyChat = true;
            _ = Task.Run(ReceiveLobbyChatLoopAsync);
        }

        public void StopLobbyChatReceiveLoop()
        {
            isReceivingLobbyChat = false;
        }

        private async Task ReceiveLobbyChatLoopAsync()
        {
            try
            {
                while (IsConnected && isReceivingLobbyChat)
                {
                    PacketData packet = await ReadPacketAsync();

                    if (packet.OpCode == 2102)
                    {
                        LobbyChatReceived?.Invoke(HandleLobbyChatMessage(packet.Payload));
                    }
                    if (packet.OpCode == 2103)
                    {
                        UpdateLobbyCount?.Invoke(HandleUpdateLobbyCount(packet.Payload));
                    }
                }
            }
            catch
            {
                isReceivingLobbyChat = false;
            }
        }

        private int HandleUpdateLobbyCount(byte[] payloadBuffer)
        {

            int count = BitConverter.ToInt32(payloadBuffer, 0);

            return count;
        }

        private RegisterResult HandleRegisterResult(byte[] payloadBuffer)
        {
            int currentOffset = 0;

            if (payloadBuffer[currentOffset] == 1)
            {
                return new RegisterResult { Success = true, Message = "Registered successfully" };
            }

            return new RegisterResult { Success = false, Message = "Username already taken" };
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

                long userID = BitConverter.ToInt64(payloadBuffer, currentOffset);

                return new AuthResult { Success = true, Message = "Logged in successfully", Username = username, UserId = userID };
            }

            return new AuthResult { Success = false, Message = "Incorrect username or password.", Username = "", UserId = 0 };
        }

        private JoinLobbyResult HandleJoinLobbyResult(byte[] payloadBuffer)
        {
            if (payloadBuffer.Length < 9 || payloadBuffer[0] != 1)
            {
                return new JoinLobbyResult { Success = false, Message = "Failed to join lobby." };
            }

            int lobbyId = BitConverter.ToInt32(payloadBuffer, 1);
            int playerCount = BitConverter.ToInt32(payloadBuffer, 5);

            return new JoinLobbyResult
            {
                Success = true,
                LobbyId = lobbyId,
                PlayerCount = playerCount,
                Message = $"Joined lobby {lobbyId}. Players: {playerCount}"
            };
        }

        private LobbyChatMessage HandleLobbyChatMessage(byte[] payloadBuffer)
        {
            int currentOffset = 0;

            ushort usernameLength = BitConverter.ToUInt16(payloadBuffer, currentOffset);
            currentOffset += 2;
            string username = Encoding.UTF8.GetString(payloadBuffer, currentOffset, usernameLength);
            currentOffset += usernameLength;

            ushort messageLength = BitConverter.ToUInt16(payloadBuffer, currentOffset);
            currentOffset += 2;
            string message = Encoding.UTF8.GetString(payloadBuffer, currentOffset, messageLength);

            return new LobbyChatMessage { Username = username, Message = message };
        }

        private async Task<PacketData> ReadPacketAsync()
        {
            var stream = tcpClient.GetStream();

            byte[] headerBuffer = new byte[6];
            await ReadExactAsync(stream, headerBuffer, 6);

            int length = BitConverter.ToInt32(headerBuffer, 0);
            int opCode = BitConverter.ToInt16(headerBuffer, 4);

            int payloadLength = length - 6;
            byte[] payloadBuffer = new byte[payloadLength];
            await ReadExactAsync(stream, payloadBuffer, payloadLength);

            return new PacketData { OpCode = opCode, Payload = payloadBuffer };
        }

        private async Task ReadExactAsync(NetworkStream stream, byte[] buffer, int bytesToRead)
        {
            int totalBytesRead = 0;
            while (totalBytesRead < bytesToRead)
            {
                int bytesRead = await stream.ReadAsync(buffer, totalBytesRead, bytesToRead - totalBytesRead);
                if (bytesRead == 0) throw new EndOfStreamException("Network connection closed unexpectedly.");
                totalBytesRead += bytesRead;
            }
        }

        public async Task LogoutAsync()
        {
            StopLobbyChatReceiveLoop();

            if (!IsConnected)
            {
                return;
            }

            await SendLogoutPacket();
            await ReceiveLogoutResultPacket();
        }

        public Task DisconnectAsync()
        {
            StopLobbyChatReceiveLoop();

            if (tcpClient.Connected)
            {
                tcpClient.Close();
            }

            tcpClient.Dispose();
            tcpClient = new TcpClient();

            return Task.CompletedTask;
        }

        private class PacketData
        {
            public int OpCode { get; set; }
            public byte[] Payload { get; set; } = Array.Empty<byte>();
        }
    }
}
