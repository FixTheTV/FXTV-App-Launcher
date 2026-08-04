using FXTVGame.Launcher.Models.Auth;
using FXTVGame.Launcher.Services;
using FXTVGame.Launcher.Services.Auth;

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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            AuthResult authResult = authService.Login(
                username_textbox.Text,
                password_textbox.Text
            );

            if (!authResult.Success)
            {
                MessageBox.Show(authResult.Message);
                password_textbox.Clear();
                password_textbox.Focus();
                return;
            }
            LoginSucceeded?.Invoke(authResult.Username);
        }

    }
}
