namespace FXTVGame.Launcher.Controls
{
    public partial class HomeControl : UserControl
    {
        public event Action? LogoutRequested;

        public HomeControl(string username)
        {
            InitializeComponent();
            welcome_label.Text = $"Hello user {username}";
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
    }
}
