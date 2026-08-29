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
        private const int S2C_LOGIN_RESULT = 2001;
        private const int S2C_REGISTER_RESULT = 2002;
        private const int S2C_LOGOUT_RESULT = 2003;
        private const int S2C_JOIN_LOBBY_RESULT = 2101;
        private const int S2C_LOBBY_CHAT = 2102;
        private const int S2C_UPDATE_LOBBY_COUNT = 2103;
        private const int S2C_LEAVE_LOBBY = 2104;
        private const int S2C_LOBBY_TAB_UPDATE = 2105;

        public static NetworkService Shared { get; } = new NetworkService();

        private readonly PacketService packetService = new PacketService();
        private readonly object packetSyncRoot = new object();
        private readonly Dictionary<int, Queue<PacketData>> waitingPackets = new Dictionary<int, Queue<PacketData>>();
        private readonly Dictionary<int, Queue<TaskCompletionSource<PacketData>>> packetWaiters = new Dictionary<int, Queue<TaskCompletionSource<PacketData>>>();
        private readonly Queue<PacketData> pendingLobbyPackets = new Queue<PacketData>();
        private TcpClient tcpClient;
        private bool isReceivingLobbyChat;
        private Task? receiveLoopTask;

        public event Action<LobbyChatMessage>? LobbyChatReceived;
        public event Action<int>? UpdateLobbyCount;

        public event Action<LobbyTab>? UpdateLobbyTab;

        private NetworkService()
        {
            tcpClient = new TcpClient();
        }

        public bool IsConnected => tcpClient.Connected;

        public static string GetLocalIPv4Address()
        {
            foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(address))
                {
                    return address.ToString();
                }
            }

            return "127.0.0.1";
        }

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
            StartReceiveLoop();
        }

        public async Task SendLoginPacket(string username, string password)
        {
            var stream = tcpClient.GetStream();
            byte[] packet = packetService.CreateLoginRequestPacket(username, password);
            await stream.WriteAsync(packet, 0, packet.Length);
        }

        public async Task SendLeaveLobbyPacket()
        {
            var stream = tcpClient.GetStream();
            byte[] packet = packetService.CreateLeaveLobbyPacket();
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

        public async Task SendLobbyReadyPacket(bool isReady)
        {
            var stream = tcpClient.GetStream();
            byte[] packet = packetService.CreateLobbyReadyPacket(isReady);
            await stream.WriteAsync(packet, 0, packet.Length);
        }

        public async Task<AuthResult> RecieveLoginResultPacket()
        {
            PacketData packet = await WaitForPacketAsync(S2C_LOGIN_RESULT);
            return HandleLoginResult(packet.Payload);
        }

        public async Task<RegisterResult> RecieveRegisterResultPacket()
        {
            PacketData packet = await WaitForPacketAsync(S2C_REGISTER_RESULT);
            return HandleRegisterResult(packet.Payload);
        }


        public async Task<JoinLobbyResult> ReceiveJoinLobbyResultPacket()
        {
            PacketData packet = await WaitForPacketAsync(S2C_JOIN_LOBBY_RESULT);
            return HandleJoinLobbyResult(packet.Payload);
        }

        public void StartLobbyChatReceiveLoop()
        {
            if (isReceivingLobbyChat)
            {
                return;
            }

            isReceivingLobbyChat = true;
            FlushPendingLobbyPackets();
        }

        public void StopLobbyChatReceiveLoop()
        {
            isReceivingLobbyChat = false;

            lock (packetSyncRoot)
            {
                pendingLobbyPackets.Clear();
            }
        }

        private void StartReceiveLoop()
        {
            if (receiveLoopTask != null && !receiveLoopTask.IsCompleted)
            {
                return;
            }

            receiveLoopTask = Task.Run(ReceiveLoopAsync);
        }

        private async Task ReceiveLoopAsync()
        {
            try
            {
                while (IsConnected)
                {
                    PacketData packet = await ReadPacketAsync();
                    RoutePacket(packet);
                }
            }
            catch (Exception ex)
            {
                FailWaitingPackets(ex);
            }
        }

        private Task<PacketData> WaitForPacketAsync(int opCode)
        {
            lock (packetSyncRoot)
            {
                if (waitingPackets.TryGetValue(opCode, out Queue<PacketData>? packets) && packets.Count > 0)
                {
                    return Task.FromResult(packets.Dequeue());
                }

                var waiter = new TaskCompletionSource<PacketData>(TaskCreationOptions.RunContinuationsAsynchronously);

                if (!packetWaiters.TryGetValue(opCode, out Queue<TaskCompletionSource<PacketData>>? waiters))
                {
                    waiters = new Queue<TaskCompletionSource<PacketData>>();
                    packetWaiters[opCode] = waiters;
                }

                waiters.Enqueue(waiter);
                return waiter.Task;
            }
        }

        private void RoutePacket(PacketData packet)
        {
            TaskCompletionSource<PacketData>? waiter = null;

            lock (packetSyncRoot)
            {
                if (packetWaiters.TryGetValue(packet.OpCode, out Queue<TaskCompletionSource<PacketData>>? waiters) && waiters.Count > 0)
                {
                    waiter = waiters.Dequeue();
                }
                else if (IsResponsePacket(packet.OpCode))
                {
                    if (!waitingPackets.TryGetValue(packet.OpCode, out Queue<PacketData>? packets))
                    {
                        packets = new Queue<PacketData>();
                        waitingPackets[packet.OpCode] = packets;
                    }

                    packets.Enqueue(packet);
                    return;
                }
                else if (IsLobbyPacket(packet.OpCode) && !isReceivingLobbyChat)
                {
                    pendingLobbyPackets.Enqueue(packet);
                    return;
                }
            }

            if (waiter != null)
            {
                waiter.SetResult(packet);
                return;
            }

            if (IsLobbyPacket(packet.OpCode))
            {
                HandleLobbyPacket(packet);
            }
        }

        private void FlushPendingLobbyPackets()
        {
            while (true)
            {
                PacketData packet;

                lock (packetSyncRoot)
                {
                    if (pendingLobbyPackets.Count == 0 || !isReceivingLobbyChat)
                    {
                        return;
                    }

                    packet = pendingLobbyPackets.Dequeue();
                }

                if (!HandleLobbyPacket(packet))
                {
                    return;
                }
            }
        }

        private void FailWaitingPackets(Exception ex)
        {
            List<TaskCompletionSource<PacketData>> waitersToFail = new List<TaskCompletionSource<PacketData>>();

            lock (packetSyncRoot)
            {
                foreach (Queue<TaskCompletionSource<PacketData>> waiters in packetWaiters.Values)
                {
                    while (waiters.Count > 0)
                    {
                        waitersToFail.Add(waiters.Dequeue());
                    }
                }

                waitingPackets.Clear();
                pendingLobbyPackets.Clear();
            }

            foreach (TaskCompletionSource<PacketData> waiter in waitersToFail)
            {
                waiter.TrySetException(ex);
            }
        }

        private bool IsResponsePacket(int opCode)
        {
            return opCode == S2C_LOGIN_RESULT || opCode == S2C_REGISTER_RESULT || opCode == S2C_LOGOUT_RESULT || opCode == S2C_JOIN_LOBBY_RESULT;
        }

        private bool IsLobbyPacket(int opCode)
        {
            return opCode == S2C_LOBBY_CHAT || opCode == S2C_UPDATE_LOBBY_COUNT || opCode == S2C_LEAVE_LOBBY || opCode == S2C_LOBBY_TAB_UPDATE;
        }

        private bool HandleLobbyPacket(PacketData packet)
        {
            switch (packet.OpCode)
            {
                case S2C_LOBBY_CHAT:
                    LobbyChatReceived?.Invoke(HandleLobbyChatMessage(packet.Payload));
                    return true;

                case S2C_UPDATE_LOBBY_COUNT:
                    UpdateLobbyCount?.Invoke(HandleUpdateLobbyCount(packet.Payload));
                    return true;

                case S2C_LOBBY_TAB_UPDATE:
                    UpdateLobbyTab?.Invoke(HandleUpdateLobbyTab(packet.Payload));
                    return true;

                case S2C_LEAVE_LOBBY:
                    isReceivingLobbyChat = false;
                    return false;

                default:
                    return true;
            }
        }

        private LobbyTab HandleUpdateLobbyTab(byte[] payloadBuffer)
        {
            int currentOffset = 0;

            ushort usernameLength = BitConverter.ToUInt16(payloadBuffer, currentOffset);
            currentOffset += 2;
            string username = Encoding.UTF8.GetString(payloadBuffer, currentOffset, usernameLength);
            currentOffset += usernameLength;

            int slot = payloadBuffer[currentOffset];
            currentOffset++;
            bool isReady = payloadBuffer.Length > currentOffset && payloadBuffer[currentOffset] == 1;

            return new LobbyTab { name = username, slot = slot, isReady = isReady };
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
            await WaitForPacketAsync(S2C_LOGOUT_RESULT);
            await DisconnectAsync();
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
            receiveLoopTask = null;
            FailWaitingPackets(new InvalidOperationException("Network connection closed."));

            return Task.CompletedTask;
        }

        private class PacketData
        {
            public int OpCode { get; set; }
            public byte[] Payload { get; set; } = Array.Empty<byte>();
        }


    }
}
