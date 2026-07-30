using FXTVGame.Launcher.Services.Auth;

namespace FXTVGame.Launcher.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnEnterLogin_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();

            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                string username = loginForm.LoginUser;
                enter_login_button.Text = $"Hello user {username}";
            }
        }
    }
}
