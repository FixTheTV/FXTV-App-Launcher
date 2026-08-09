using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
namespace FXTVGame.Launcher.Services
{
    internal class PacketService
    {
        
        private const int LOGIN_REQ_HEADER_LENGTH = 8;

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
    }
}
