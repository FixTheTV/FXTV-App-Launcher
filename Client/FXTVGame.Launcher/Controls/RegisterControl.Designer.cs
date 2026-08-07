namespace FXTVGame.Launcher.Controls
{
    partial class RegisterControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            login_button = new Button();
            username_label = new Label();
            password_label = new Label();
            retype_password_label = new Label();
            username_textbox = new TextBox();
            password_textbox = new TextBox();
            retype_password_textbox = new TextBox();
            username_warning_label = new Label();
            retype_pass_warn_label = new Label();
            pass_warn_label = new Label();
            SuspendLayout();
            // 
            // login_button
            // 
            login_button.Location = new Point(47, 351);
            login_button.Name = "login_button";
            login_button.Size = new Size(234, 40);
            login_button.TabIndex = 4;
            login_button.Text = "REGISTER";
            login_button.UseVisualStyleBackColor = true;
            login_button.Click += btnRegister_Click;
            // 
            // username_label
            // 
            username_label.AutoSize = true;
            username_label.Location = new Point(47, 43);
            username_label.Name = "username_label";
            username_label.Size = new Size(106, 20);
            username_label.TabIndex = 0;
            username_label.Text = "Your username";
            // 
            // password_label
            // 
            password_label.AutoSize = true;
            password_label.Location = new Point(47, 151);
            password_label.Name = "password_label";
            password_label.Size = new Size(70, 20);
            password_label.TabIndex = 2;
            password_label.Text = "Password";
            // 
            // retype_password_label
            // 
            retype_password_label.Location = new Point(47, 259);
            retype_password_label.Name = "retype_password_label";
            retype_password_label.Size = new Size(122, 23);
            retype_password_label.TabIndex = 0;
            retype_password_label.Text = "Retype password";
            // 
            // username_textbox
            // 
            username_textbox.Location = new Point(47, 66);
            username_textbox.Name = "username_textbox";
            username_textbox.PlaceholderText = "Enter username";
            username_textbox.Size = new Size(234, 27);
            username_textbox.TabIndex = 1;
            // 
            // password_textbox
            // 
            password_textbox.Location = new Point(47, 174);
            password_textbox.Name = "password_textbox";
            password_textbox.PlaceholderText = "Enter password";
            password_textbox.Size = new Size(234, 27);
            password_textbox.TabIndex = 3;
            password_textbox.UseSystemPasswordChar = true;
            // 
            // retype_password_textbox
            // 
            retype_password_textbox.Location = new Point(47, 282);
            retype_password_textbox.Name = "retype_password_textbox";
            retype_password_textbox.PlaceholderText = "Retype password";
            retype_password_textbox.Size = new Size(234, 27);
            retype_password_textbox.TabIndex = 0;
            retype_password_textbox.UseSystemPasswordChar = true;
            // 
            // username_warning_label
            // 
            username_warning_label.AutoSize = true;
            username_warning_label.Location = new Point(47, 103);
            username_warning_label.Name = "username_warning_label";
            username_warning_label.Size = new Size(0, 20);
            username_warning_label.TabIndex = 5;
            // 
            // retype_pass_warn_label
            // 
            retype_pass_warn_label.AutoSize = true;
            retype_pass_warn_label.Location = new Point(47, 319);
            retype_pass_warn_label.Name = "retype_pass_warn_label";
            retype_pass_warn_label.Size = new Size(0, 20);
            retype_pass_warn_label.TabIndex = 6;
            // 
            // pass_warn_label
            // 
            pass_warn_label.AutoSize = true;
            pass_warn_label.Location = new Point(47, 211);
            pass_warn_label.Name = "pass_warn_label";
            pass_warn_label.Size = new Size(0, 20);
            pass_warn_label.TabIndex = 7;
            // 
            // RegisterControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SteelBlue;
            Controls.Add(pass_warn_label);
            Controls.Add(retype_pass_warn_label);
            Controls.Add(username_warning_label);
            Controls.Add(password_textbox);
            Controls.Add(login_button);
            Controls.Add(retype_password_textbox);
            Controls.Add(retype_password_label);
            Controls.Add(username_textbox);
            Controls.Add(password_label);
            Controls.Add(username_label);
            Name = "RegisterControl";
            Size = new Size(328, 450);
            ResumeLayout(false);
            PerformLayout();
        }
        private Button login_button;
        private Label username_label;
        private Label password_label;
        private Label retype_password_label;
        private TextBox username_textbox;
        private TextBox password_textbox;
        private TextBox retype_password_textbox;
        #endregion

        private Label username_warning_label;
        private Label retype_pass_warn_label;
        private Label pass_warn_label;
    }
}
