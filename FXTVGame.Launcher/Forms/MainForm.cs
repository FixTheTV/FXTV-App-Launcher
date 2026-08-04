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
            ResizeToScreen(screen.Size);
            contentPanel.Controls.Clear();
            screen.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(screen);
        }

        private void ResizeToScreen(Size targetClientSize)
        {
            if (WindowState != FormWindowState.Normal)
            {
                ClientSize = targetClientSize;
                return;
            }

            Point currentCenter = new Point(
                Left + Width / 2,
                Top + Height / 2
            );

            ClientSize = targetClientSize;

            Left = currentCenter.X - Width / 2;
            Top = currentCenter.Y - Height / 2;

            Rectangle workingArea = Screen.FromControl(this).WorkingArea;

            if (Left < workingArea.Left)
            {
                Left = workingArea.Left;
            }

            if (Top < workingArea.Top)
            {
                Top = workingArea.Top;
            }

            if (Right > workingArea.Right)
            {
                Left = workingArea.Right - Width;
            }

            if (Bottom > workingArea.Bottom)
            {
                Top = workingArea.Bottom - Height;
            }
        }
    }
}

