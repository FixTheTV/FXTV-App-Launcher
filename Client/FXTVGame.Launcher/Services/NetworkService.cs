using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace FXTVGame.Launcher.Services
{
    internal class NetworkService
    {
        public readonly TcpClient tcpClient;
        public NetworkService()
        {
            tcpClient = new TcpClient();
        }

        public async Task ConnectAsync(IPAddress ip, int port)
        {
            try { await tcpClient.ConnectAsync(ip, port); } catch { };
        }
        public async Task ConnectAsync(string ip, int port)
        {
            var convertedIP = IPAddress.Parse(ip);
            try { await tcpClient.ConnectAsync(convertedIP, port); } catch { };
        }

        public async Task SendLoginPacket(string username, string password)
        {
            var stream = tcpClient.GetStream();
            
                if (stream == null) return;

                byte[] userBytes = Encoding.UTF8.GetBytes(username);
                byte[] passBytes = Encoding.UTF8.GetBytes(password);

                if (userBytes.Length > 255 || passBytes.Length > 255)
                {
                    throw new ArgumentException("Tài khoản hoặc mật khẩu quá dài!");
                }

                int totalLength = 4 + 2 + 1 + userBytes.Length + 1 + passBytes.Length;
                byte[] packet = new byte[totalLength];

                Array.Copy(BitConverter.GetBytes(totalLength), 0, packet, 0, 4);
                Array.Copy(BitConverter.GetBytes((short)1001), 0, packet, 4, 2);

                int currentOffset = 6;
                packet[currentOffset] = (byte)userBytes.Length;
                currentOffset += 1;

                Array.Copy(userBytes, 0, packet, currentOffset, userBytes.Length);
                currentOffset += userBytes.Length;

                packet[currentOffset] = (byte)passBytes.Length;
                currentOffset += 1;

                Array.Copy(passBytes, 0, packet, currentOffset, passBytes.Length);

                await stream.WriteAsync(packet, 0, packet.Length);
            
            


        }

        public async Task RecieveLoginResultPacket()
        {
            var stream = tcpClient.GetStream();
            
            byte[] headerBuffer= new byte[6];
            int bytesRead = await stream.ReadAsync(headerBuffer, 0, headerBuffer.Length);

            if (bytesRead < 6) return;

            int length = BitConverter.ToInt32(headerBuffer, 0);
            int opCode = BitConverter.ToInt16(headerBuffer, 4);

            byte[] payloadBuffer = new byte[length - 6];
            bytesRead = await stream.ReadAsync(payloadBuffer, 0, payloadBuffer.Length);


            await HandleLoginResult(payloadBuffer);
               
            
        }

        public async Task HandleLoginResult(byte[] payloadBuffer)
        {
            if (payloadBuffer[0] == 1)
            {
                MessageBox.Show("YEAH");
            }
            else
            {
                MessageBox.Show("NAH NAH");
            }
        }
    }
}
