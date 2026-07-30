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
                enter_login_button.Text = "MISSION COMPLETE";
            }
        }
    }
}