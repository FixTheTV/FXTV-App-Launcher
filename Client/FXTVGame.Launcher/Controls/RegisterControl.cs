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
    public partial class RegisterControl : UserControl
    {
        public event Action? RegisterSucceed;
        private readonly RegisterService regService = new RegisterService();
        public RegisterControl()
        {
            InitializeComponent();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            pass_warn_label.Text = "";
            retype_pass_warn_label.Text = "";
            username_warning_label.Text = "";

            string usernameField = username_textbox.Text;
            string passwordField = password_textbox.Text;
            string repasswordField = retype_password_textbox.Text;

            var check = new Models.Register.RegisterResult();

            try
            {
                check = await regService.ValidateRegForm(usernameField,passwordField,repasswordField);
            }
            catch
            {
                MessageBox.Show("Cannot connect to server.");
                return;
            }

            if (check.Success)
            {
                MessageBox.Show(check.Message);
                RegisterSucceed?.Invoke();
            }
            else
            {
                switch (check.FormSlot)
                {
                    
                    case 0:
                        username_warning_label.Text = check.Message;
                        username_textbox.Clear();
                        username_textbox.Focus();
                        break;
                    case 1:
                        pass_warn_label.Text = check.Message;
                        password_textbox.Clear();
                        password_textbox.Focus();
                        MessageBox.Show(check.Message);
                        break;
                    case 2:
                        retype_pass_warn_label.Text = check.Message;
                        retype_password_textbox.Clear();
                        retype_password_textbox.Focus();
                        MessageBox.Show(check.Message);
                        break;
                }
            }


        }


    }
}
