using FXTVGame.Launcher.Models.Auth;
using FXTVGame.Launcher.Services.Database;
using System.Net;
using System.Text;

namespace FXTVGame.Launcher.Services.Auth
{
    public class AuthService
    {
        private readonly DatabaseService databaseService = new DatabaseService();
        private readonly NetworkService networkService = new NetworkService();
        public AuthService()
        {
        }

        public AuthResult Login(string username, string password)
        {
            username = username.Trim();

            _ = SendLoginAsync(username,password);
            var checkUserExists = databaseService.CheckIfUserExistsAndGetId(username);

            if (checkUserExists.SearchResult)
            {
                if (databaseService.CheckPassword(checkUserExists.UserId, password))
                {
                    return new AuthResult
                    {
                        Success = true,
                        Message = "Login successful.",
                        UserId = checkUserExists.UserId,
                        Username = username
                    };
                }
                return new AuthResult
                {
                    Success = false,
                    Message = "Incorrect username or password."
                };
                
            }
            else
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "You don't have an existing account."
                };
            }
                      
        }

        public async void ConnectBoisss()
        {
            await networkService.ConnectAsync("192.168.1.188",12345);
        }

        public async Task SendLoginAsync(string username, string password)
        {
            var stream = networkService.tcpClient.GetStream();

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
    }
}
