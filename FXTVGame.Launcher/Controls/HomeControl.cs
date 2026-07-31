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
            LogoutRequested?.Invoke();
        }
    }
}

