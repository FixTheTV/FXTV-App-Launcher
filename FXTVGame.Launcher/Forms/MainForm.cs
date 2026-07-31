using FXTVGame.Launcher.Controls;

namespace FXTVGame.Launcher.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ShowMain();
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
            homeControl.LogoutRequested += ShowMain;

            ShowScreen(homeControl);
        }

        private void ShowMain()
        {
            var mainControl = new MainControl();
            mainControl.GoToLoginRequested += ShowLogin;
            mainControl.GoToRegRequested += ShowRegister;

            ShowScreen(mainControl);
        }

        private void ShowRegister()
        {
            var registerControl = new RegisterControl();
            registerControl.RegisterSucceed += ShowMain;

            ShowScreen(registerControl);
        }

        private void ShowScreen(UserControl screen)
        {
            contentPanel.Controls.Clear();
            screen.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(screen);
        }
    }
}

