using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FXTVGame.Launcher.Controls
{
    public partial class LogoutConfirmationDialog : UserControl
    {
        public event Action? ConfirmLogOut;

        public LogoutConfirmationDialog()
        {
            InitializeComponent();
        }

        private void logout_confirm_button_Click(object sender, EventArgs e)
        {
            ConfirmLogOut?.Invoke();
        }

        private void logout_cancel_button_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
