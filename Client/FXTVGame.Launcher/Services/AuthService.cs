using FXTVGame.Launcher.Models.Auth;
using FXTVGame.Launcher.Services.Database;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Windows.Forms.VisualStyles;

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
            await networkService.SendLoginPacket(username, password);

            Console.Write("I RAN");
            await networkService.RecieveLoginResultPacket();
        }

    }
}
