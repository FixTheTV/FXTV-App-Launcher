using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FXTVGame.Launcher.Controls
{
    public partial class MainControl : UserControl
    {
        public event Action? GoToLoginRequested;
        public event Action? GoToRegRequested;
        public MainControl()
        {
            InitializeComponent();
        }

        private void btnGoToLogin_Click(object sender, EventArgs e)
        {
            GoToLoginRequested?.Invoke();
        }
        private void btnGoToRegister_Click(object sender, EventArgs e)
        {
            GoToRegRequested?.Invoke();
        }
    }
}
