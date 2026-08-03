using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FXTVGame.Launcher.Services.Database;
using FXTVGame.Launcher.Services.Register;

namespace FXTVGame.Launcher.Controls
{
    public partial class RegisterControl : UserControl
    {
        public event Action? RegisterSucceed;
        public RegisterControl()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            pass_warn_label.Text = "";
            retype_pass_warn_label.Text = "";
            username_warning_label.Text = "";
            RegisterService regService = new RegisterService();

            string usernameField = username_textbox.Text;
            string passwordField = password_textbox.Text;
            string repasswordField = retype_password_textbox.Text;

            var check = regService.ValidateRegForm(usernameField,passwordField,repasswordField);

            if (check.Success)
            {
                DatabaseService database = new DatabaseService();
                bool userAdded = database.AddUser(usernameField, passwordField);

                if (userAdded)
                {
                    MessageBox.Show(check.Message);
                    RegisterSucceed?.Invoke();
                }
                else
                {
                    username_warning_label.Text = "Username already taken.";
                    username_textbox.Clear();
                    username_textbox.Focus();
                }
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
                        break;
                    case 2: 
                        retype_pass_warn_label.Text = check.Message;
                        retype_password_textbox.Clear();
                        retype_password_textbox.Focus();
                        break;
                }
            }
            
            
        }

    }
}
