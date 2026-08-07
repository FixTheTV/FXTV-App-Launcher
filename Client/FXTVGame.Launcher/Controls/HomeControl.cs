using FXTVGame.Launcher.Services;

namespace FXTVGame.Launcher.Controls
{
    public partial class HomeControl : UserControl
    {
        private readonly LauncherPreferenceService preferenceService = new LauncherPreferenceService();

        public event Action? LogoutRequested;
        public event Action? GoToSetting;
        public event Action? PlayOnline;

        public HomeControl(string username)
        {
            InitializeComponent();
            Size = preferenceService.LoadSettings().ScreenSize;
            welcome_label.Text = $"Welcome {username} !";
            CenterContent();
        }

        private void logout_button_Click(object sender, EventArgs e)
        {
            var logoutConfirmationDialog = new LogoutConfirmationDialog();

            logoutConfirmationDialog.ConfirmLogOut += OnLogoutConfirmed;

            logoutConfirmationDialog.Location = new Point(
                (Width - logoutConfirmationDialog.Width) / 2,
                (Height - logoutConfirmationDialog.Height) / 2
            );

            Controls.Add(logoutConfirmationDialog);
            logoutConfirmationDialog.BringToFront();
        }

        private void OnLogoutConfirmed()
        {
            LogoutRequested?.Invoke();
        }

        private void settings_button_Click(object sender, EventArgs e)
        {
            GoToSetting?.Invoke();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            CenterContent();
        }

        private void CenterContent()
        {
            if (settings_button == null)
            {
                return;
            }

            int buttonGap = 20;
            int totalButtonHeight = offline_play_button.Height + online_play_button.Height + settings_button.Height + buttonGap * 2;
            int buttonX = (Width - offline_play_button.Width) / 2;
            int buttonY = (Height - totalButtonHeight) / 2;

            welcome_label.Location = new Point((Width - welcome_label.Width) / 2, Math.Max(48, buttonY - 96));

            offline_play_button.Location = new Point(buttonX, buttonY);
            online_play_button.Location = new Point(buttonX, buttonY + offline_play_button.Height + buttonGap);
            settings_button.Location = new Point(buttonX, online_play_button.Bottom + buttonGap);

            logout_button.Location = new Point(
                Width - logout_button.Width - 48,
                Height - logout_button.Height - 48
            );
        }

        private void online_play_button_Click(object sender, EventArgs e)
        {
            PlayOnline?.Invoke();
        }
    }
}
