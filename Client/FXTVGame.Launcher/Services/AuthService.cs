using FXTVGame.Launcher.Models.Auth;
using FXTVGame.Launcher.Services;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace FXTVGame.Launcher.Services.Auth
{
    public class AuthService
    {
        private readonly NetworkService networkService = NetworkService.Shared;

        public AuthResult authResult = new AuthResult { Success = false, Message = "Failed to Login", UserId = 0, Username = "placeholder" };
        public AuthService()
        {
        }

        public async Task Login(string username, string password)
        {
            username = username.Trim();

            await SendLoginAsync(username,password);          
        }

        public async Task ConnectBoisss()
        {
            await networkService.ConnectAsync("192.168.1.5",12345);   
        }

        public async Task SendLoginAsync(string username, string password)
        {
            await networkService.SendLoginPacket(username, password);
            this.authResult = await networkService.RecieveLoginResultPacket();
        }

    }
}
