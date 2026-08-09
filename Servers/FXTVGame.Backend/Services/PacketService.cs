using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace FXTVGame.Backend.Services
{
    internal class PacketService
    {
        private const int HEADER_LENGTH = 6; 


        public byte[] CreatePacketHeader(int totalPackgetlength, int opCode)
        {
            byte[] packetHeader = new byte[HEADER_LENGTH];

            Array.Copy(BitConverter.GetBytes(totalPackgetlength), 0, packetHeader, 0 ,4);
            Array.Copy(BitConverter.GetBytes((short)opCode), 0, packetHeader, 4, 2);

            return packetHeader;

            //packet should looks like zis [0 - 3: Length] [4 - 5: OpCode]
        }
        
        public byte[] CreateLoginRequestPacket(string username, string password)
        {
            int usernameLength = Encoding.UTF8.GetByteCount(username);
            int passwordLength = Encoding.UTF8.GetByteCount(password);

            int totalLength = HEADER_LENGTH + 2 + usernameLength + passwordLength;
            byte[] loginPacket = new byte[totalLength];


            Array.Copy(CreatePacketHeader(totalLength, 1001), loginPacket, 6);

            int currentOffset = 6;

            loginPacket[currentOffset] = (byte)usernameLength;
            ++currentOffset;

            Array.Copy(Encoding.UTF8.GetBytes(username), 0, loginPacket, currentOffset, usernameLength);
            currentOffset += usernameLength;

            loginPacket[currentOffset] = (byte)passwordLength;
            ++currentOffset;

            Array.Copy(Encoding.UTF8.GetBytes(password), 0, loginPacket, currentOffset, passwordLength);

            return loginPacket;

            //packet should looks like zis [0 - 3 Length] [4 - 5 OPCode] [6 Username Length] [7 - ... Username] [... - ... + 1 Password Length] [... - ... Password]
        }
        public byte[] CreateLoginResultPacket(bool res, string username, Int64 userID = 0)
        {
            int totalLength = HEADER_LENGTH;

            if (res)
            {
                int usernameLength = Encoding.UTF8.GetByteCount(username);
                totalLength = totalLength + usernameLength + 10;
                byte[] loginResPacket = new byte[totalLength];

                Array.Copy(CreatePacketHeader(totalLength, 2001), loginResPacket, HEADER_LENGTH);

                int currentOffset = HEADER_LENGTH;

                loginResPacket[currentOffset] = (byte)1;
                ++currentOffset;
                loginResPacket[currentOffset] = (byte)usernameLength;
                ++currentOffset;
               

                Array.Copy(Encoding.UTF8.GetBytes(username), 0, loginResPacket, currentOffset, usernameLength);
                currentOffset += usernameLength;

                Array.Copy(BitConverter.GetBytes(userID), 0, loginResPacket, currentOffset, 8);

                return loginResPacket;

            }
            else
            {
                ++totalLength;
                byte[] loginResPacket = new byte[totalLength];

                Array.Copy(CreatePacketHeader(totalLength, 2001), loginResPacket, HEADER_LENGTH);

                loginResPacket[totalLength-1] = (byte)0;
                return loginResPacket;

            }

            //packet if succ [0 - 3 Length] [4 - 5 OPCode] [6 Result] [7 Username Length] [8 - ... Username] [... - ... + 8 UserID]
            //packet if fail [0 - 3 Length] [4 - 5 OPCode] [6 Result]
        }
        public byte[] CreateRegisterRequestPacket(string username, string password)
        {
            int usernameLength = Encoding.UTF8.GetByteCount(username);
            int passwordLength = Encoding.UTF8.GetByteCount(password);

            int totalLength = HEADER_LENGTH + 2 + usernameLength + passwordLength;
            byte[] loginPacket = new byte[totalLength];


            Array.Copy(CreatePacketHeader(totalLength, 1002), loginPacket, 6);

            int currentOffset = 6;

            loginPacket[currentOffset] = (byte)usernameLength;
            ++currentOffset;

            Array.Copy(Encoding.UTF8.GetBytes(username), 0, loginPacket, currentOffset, usernameLength);
            currentOffset += usernameLength;

            loginPacket[currentOffset] = (byte)passwordLength;
            ++currentOffset;

            Array.Copy(Encoding.UTF8.GetBytes(password), 0, loginPacket, currentOffset, passwordLength);

            return loginPacket;

            //packet should looks like zis [0 - 3 Length] [4 - 5 OPCode] [6 Username Length] [7 - ... Username] [... - ... + 1 Password Length] [... - ... Password]
        }
        public byte[] CreateRegisterResultPacket(bool res, string username = "failed")
        {
            int totalLength = HEADER_LENGTH;

            if (res)
            {
                int usernameLength = Encoding.UTF8.GetByteCount(username);
                totalLength = totalLength + usernameLength + 2;
                byte[] registerResPacket = new byte[totalLength];

                Array.Copy(CreatePacketHeader(totalLength, 2002), registerResPacket, HEADER_LENGTH);

                int currentOffset = HEADER_LENGTH;

                registerResPacket[currentOffset] = (byte)1;
                ++currentOffset;
                registerResPacket[currentOffset] = (byte)usernameLength;
                ++currentOffset;


                Array.Copy(Encoding.UTF8.GetBytes(username), 0, registerResPacket, currentOffset, usernameLength);
                currentOffset += usernameLength;


                return registerResPacket;

            }
            else
            {
                ++totalLength;
                byte[] registerResPacket = new byte[totalLength];

                Array.Copy(CreatePacketHeader(totalLength, 2002), registerResPacket, HEADER_LENGTH);

                registerResPacket[totalLength - 1] = (byte)0;
                return registerResPacket;

            }

            //packet if succ [0 - 3 Length] [4 - 5 OPCode] [6 Result] [7 Username Length] [8 - ... Username] [... - ... + 8 UserID]
            //packet if fail [0 - 3 Length] [4 - 5 OPCode] [6 Result]
        }
    }
}
