using FXTVGame.Launcher.Controls;
using FXTVGame.Launcher.Models;
using FXTVGame.Launcher.Services;

namespace FXTVGame.Launcher.Forms
{
    public partial class MainForm : Form
    {
        private readonly LauncherPreferenceService preferenceService = new LauncherPreferenceService();
        private string? CurrentUserSession;

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
            CurrentUserSession = username;
            ShowHomeFromSession();
        }

        private void ShowHomeFromSession()
        {
            if (string.IsNullOrWhiteSpace(CurrentUserSession))
            {
                ShowMain();
                return;
            }

            var homeControl = new HomeControl(CurrentUserSession);
            homeControl.LogoutRequested += Logout;
            homeControl.GoToSetting += ShowSetting;
            homeControl.PlayOnline += ShowFindLobby;
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
        private void ShowSetting()
        {
            var settingControl = new SettingControl();
            settingControl.SettingApplied += ApplyDisplaySettings;
            settingControl.BackToHome += ShowHomeFromSession;

            ShowScreen(settingControl);
        }

        private void Logout()
        {
            CurrentUserSession = null;
            ShowMain();
        }
        private void ShowFindLobby()
        {
            var findLobbyControl = new FindLobbyControl();
            ShowScreen(findLobbyControl);
        }


        private void ShowScreen(UserControl screen)
        {
            if (UsesDisplaySettings(screen))
            {
                ApplyDisplaySettings(preferenceService.LoadSettings());
            }
            else
            {
                ApplyWindowedSize(screen.Size);
            }

            contentPanel.Controls.Clear();
            screen.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(screen);
        }
        private bool UsesDisplaySettings(UserControl screen)
        {
            return screen is HomeControl || screen is SettingControl;
        }
        private void ApplyDisplaySettings(LauncherDisplaySettings settings)
        {
            SuspendLayout();

            if (settings.WindowMode == "Fullscreen")
            {
                WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.None;
                Bounds = Screen.FromControl(this).Bounds;
            }
            else if (settings.WindowMode == "Borderless Windowed")
            {
                WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.None;
                Bounds = Screen.FromControl(this).WorkingArea;
            }
            else
            {
                FormBorderStyle = FormBorderStyle.FixedSingle;
                ApplyWindowedSize(settings.ScreenSize);
            }

            ResumeLayout();
        }
        private void ApplyWindowedSize(Size targetClientSize)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;

            if (WindowState != FormWindowState.Normal)
            {
                WindowState = FormWindowState.Normal;
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

