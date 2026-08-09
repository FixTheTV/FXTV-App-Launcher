using FXTVGame.Launcher.Models.Auth;
using FXTVGame.Launcher.Services;
using FXTVGame.Launcher.Services.Auth;
using System.Net.NetworkInformation;

namespace FXTVGame.Launcher.Controls
{
    public partial class LoginControl : UserControl
    {
        private readonly AuthService authService = new AuthService();


        public event Action<string>? LoginSucceeded;

        public LoginControl()
        {
            InitializeComponent();
        }

        public async void btnLogin_Click(object arg, EventArgs e)
        {
            await authService.ConnectBoisss();

           await authService.Login(
                username_textbox.Text,
                password_textbox.Text
            );


            if (!authService.authResult.Success)
            {
                MessageBox.Show(authService.authResult.Message);
                password_textbox.Clear();
                password_textbox.Focus();
                return;
            }
            LoginSucceeded?.Invoke(authService.authResult.Username);
        }


    }
}
