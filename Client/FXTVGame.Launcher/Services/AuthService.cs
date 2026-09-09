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

            await SendLoginAsync(username,password).ConfigureAwait(false);          
        }

        public async Task ConnectBoisss()
        {
            // This picks this PC's LAN IPv4, for example 192.168.x.x.
            // Use it when the backend server is running on the same PC as the launcher.
            string serverIpAddress = NetworkService.GetLocalIPv4Address();

            // If the backend server is running on another PC, comment the auto line above
            // and uncomment this manual LAN IP line instead.
            //string serverIpAddress = "192.168.2.142";

            await networkService.ConnectAsync(serverIpAddress, 12345);   
        }

        public async Task SendLoginAsync(string username, string password)
        {
            await networkService.SendLoginPacket(username, password);
            this.authResult = await networkService.RecieveLoginResultPacket();
        }

    }
}
