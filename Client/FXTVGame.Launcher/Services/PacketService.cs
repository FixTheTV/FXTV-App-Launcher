using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;


namespace FXTVGame.Launcher.Services
{
    internal class PacketService
    {
        
        private const int LOGIN_REQ_HEADER_LENGTH = 8;
        private const int REG_REQ_HEADER_LENGTH = 8;
        private const int HEADER_LENGTH = 6;
        private const int C2S_LOGOUT = 1003;
        private const int C2S_LEAVELOBBY = 1104;
        private const int C2S_JOIN_LOBBY = 1101;
        private const int C2S_LOBBY_CHAT = 1102;
        private const int C2S_LOBBY_READY = 1103;

        private const int C2S_Pong = 1000;

        public byte[] CreatePongPacket()
        {
            byte[] packet = new byte[6];

            Array.Copy(CreatePacketHeader(6, C2S_Pong), packet, 6);

            return packet;
        }
        public byte[] CreatePacketHeader(int totalPackgetlength, int opCode)
        {
            byte[] packetHeader = new byte[6];

            Array.Copy(BitConverter.GetBytes(totalPackgetlength), 0, packetHeader, 0, 4);
            Array.Copy(BitConverter.GetBytes((short)opCode), 0, packetHeader, 4, 2);

            return packetHeader;
        }

        public byte[] CreateLoginRequestPacket(string username, string password)
        {
            int usernameLengthInBytes = Encoding.UTF8.GetByteCount(username);
            int passwordLengthInBytes = Encoding.UTF8.GetByteCount(password);
            int totalLength = LOGIN_REQ_HEADER_LENGTH + usernameLengthInBytes + passwordLengthInBytes;
            byte[] loginPacket = new byte[totalLength];

            Array.Copy(BitConverter.GetBytes(totalLength), 0, loginPacket, 0, 4);
            Array.Copy(BitConverter.GetBytes((short)1001), 0, loginPacket, 4, 2);

            int currentOffset = 6;

            loginPacket[currentOffset] = (byte)usernameLengthInBytes;
            ++currentOffset;

            Array.Copy(Encoding.UTF8.GetBytes(username), 0, loginPacket, currentOffset, usernameLengthInBytes);
            currentOffset += usernameLengthInBytes;

            loginPacket[currentOffset] = (byte)passwordLengthInBytes;
            ++currentOffset;

            Array.Copy(Encoding.UTF8.GetBytes(password), 0, loginPacket, currentOffset, passwordLengthInBytes);

            return loginPacket;
        }

        public byte[] CreateRegisterRequestPacket(string username, string password)
        {
            int usernameLengthInBytes = Encoding.UTF8.GetByteCount(username);
            int passwordLengthInBytes = Encoding.UTF8.GetByteCount(password);
            int totalLength = REG_REQ_HEADER_LENGTH + usernameLengthInBytes + passwordLengthInBytes;
            byte[] registerPacket = new byte[totalLength];

            Array.Copy(BitConverter.GetBytes(totalLength), 0, registerPacket, 0, 4);
            Array.Copy(BitConverter.GetBytes((short)1002), 0, registerPacket, 4, 2);

            int currentOffset = 6;

            registerPacket[currentOffset] = (byte)usernameLengthInBytes;
            ++currentOffset;

            Array.Copy(Encoding.UTF8.GetBytes(username), 0, registerPacket, currentOffset, usernameLengthInBytes);
            currentOffset += usernameLengthInBytes;

            registerPacket[currentOffset] = (byte)passwordLengthInBytes;
            ++currentOffset;

            Array.Copy(Encoding.UTF8.GetBytes(password), 0, registerPacket, currentOffset, passwordLengthInBytes);

            return registerPacket;
        }

        public byte[] CreateLogoutRequestPacket()
        {
            int totalLength = HEADER_LENGTH;
            byte[] logoutPacket = new byte[totalLength];

            Array.Copy(CreatePacketHeader(totalLength, C2S_LOGOUT), logoutPacket, HEADER_LENGTH);

            return logoutPacket;
        }

        public byte[] CreateLeaveLobbyPacket()
        {
            int totalLength = HEADER_LENGTH;
            byte[] leaveLobbyPacket = new byte[totalLength];

            Array.Copy(CreatePacketHeader(totalLength, C2S_LEAVELOBBY), leaveLobbyPacket, HEADER_LENGTH);

            return leaveLobbyPacket;
        }

        public byte[] CreateJoinLobbyRequestPacket(int lobbyId)
        {
            int totalLength = HEADER_LENGTH + 4;
            byte[] joinLobbyPacket = new byte[totalLength];

            Array.Copy(CreatePacketHeader(totalLength, C2S_JOIN_LOBBY), joinLobbyPacket, HEADER_LENGTH);
            Array.Copy(BitConverter.GetBytes(lobbyId), 0, joinLobbyPacket, HEADER_LENGTH, 4);

            return joinLobbyPacket;
        }

        public byte[] CreateLobbyChatPacket(string message)
        {
            int messageLength = Encoding.UTF8.GetByteCount(message);
            int totalLength = HEADER_LENGTH + 2 + messageLength;
            byte[] chatPacket = new byte[totalLength];

            Array.Copy(CreatePacketHeader(totalLength, C2S_LOBBY_CHAT), chatPacket, HEADER_LENGTH);

            int currentOffset = HEADER_LENGTH;
            Array.Copy(BitConverter.GetBytes((ushort)messageLength), 0, chatPacket, currentOffset, 2);
            currentOffset += 2;
            Array.Copy(Encoding.UTF8.GetBytes(message), 0, chatPacket, currentOffset, messageLength);

            return chatPacket;
        }

        public byte[] CreateLobbyReadyPacket(bool isReady)
        {
            int totalLength = HEADER_LENGTH + 1;
            byte[] readyPacket = new byte[totalLength];

            Array.Copy(CreatePacketHeader(totalLength, C2S_LOBBY_READY), readyPacket, HEADER_LENGTH);
            readyPacket[HEADER_LENGTH] = isReady ? (byte)1 : (byte)0;

            return readyPacket;
        }
    }
}
