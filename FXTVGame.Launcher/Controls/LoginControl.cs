using FXTVGame.Launcher.Models.Auth;
using FXTVGame.Launcher.Services;
using FXTVGame.Launcher.Services.Auth;

namespace FXTVGame.Launcher.Controls
{
    public partial class LoginControl : UserControl
    {
        private readonly AuthService authService = new AuthService();
        private readonly RememberMeService rememberMeService = new RememberMeService();

        public event Action<string>? LoginSucceeded;

        public LoginControl()
        {
            InitializeComponent();
            LoadRememberedUser();
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

            SaveRememberedUser(authResult.Username);
            LoginSucceeded?.Invoke(authResult.Username);
        }

        private void LoadRememberedUser()
        {
            string username = rememberMeService.LoadUsername();

            if (string.IsNullOrWhiteSpace(username))
            {
                return;
            }

            username_textbox.Text = username;
            remember_me_checkbox.Checked = true;
            password_textbox.Focus();
        }

        private void SaveRememberedUser(string username)
        {
            if (remember_me_checkbox.Checked)
            {
                rememberMeService.SaveUsername(username);
                return;
            }

            rememberMeService.ClearUsername();
        }
    }
}
