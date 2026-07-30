using FXTVGame.Launcher.Models.Auth;
using FXTVGame.Launcher.Services.Auth;

namespace FXTVGame.Launcher.Forms
{
    public partial class LoginForm : Form
    {
        private readonly AuthService authService = new AuthService();
        public string LoginUser { get; private set; } = string.Empty;

        public LoginForm()
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

            MessageBox.Show(authResult.Message);
            LoginUser = authResult.Username;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
