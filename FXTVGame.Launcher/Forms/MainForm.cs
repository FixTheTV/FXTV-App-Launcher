using FXTVGame.Launcher.Controls;

namespace FXTVGame.Launcher.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ShowLogin();
        }

        private void ShowLogin()
        {
            var loginControl = new LoginControl();
            loginControl.LoginSucceeded += ShowHome;

            ShowScreen(loginControl);
        }

        private void ShowHome(string username)
        {
            var homeControl = new HomeControl(username);
            homeControl.LogoutRequested += ShowLogin;

            ShowScreen(homeControl);
        }

        private void ShowScreen(UserControl screen)
        {
            contentPanel.Controls.Clear();
            screen.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(screen);
        }
    }
}

