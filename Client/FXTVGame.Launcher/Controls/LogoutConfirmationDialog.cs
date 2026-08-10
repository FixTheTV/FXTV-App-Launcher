using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FXTVGame.Launcher.Services;

namespace FXTVGame.Launcher.Controls
{
    public partial class LogoutConfirmationDialog : UserControl
    {
        private readonly NetworkService networkService = new NetworkService();
        public event Action? ConfirmLogOut;

        public LogoutConfirmationDialog()
        {
            InitializeComponent();
        }

        private async void logout_confirm_button_Click(object sender, EventArgs e)
        {
            await networkService.DisconnectAsync();
            ConfirmLogOut?.Invoke();
        }

        private async void logout_cancel_button_Click(object sender, EventArgs e)
        {
            this.Dispose();   
        }
    }
}
