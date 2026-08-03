namespace FXTVGame.Launcher.Controls
{
    partial class LoginControl
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
            username_textbox = new TextBox();
            password_textbox = new TextBox();
            remember_me_checkbox = new CheckBox();
            SuspendLayout();
            // 
            // login_button
            // 
            login_button.Location = new Point(297, 371);
            login_button.Name = "login_button";
            login_button.Size = new Size(234, 40);
            login_button.TabIndex = 5;
            login_button.Text = "LOGIN";
            login_button.UseVisualStyleBackColor = true;
            login_button.Click += btnLogin_Click;
            // 
            // username_label
            // 
            username_label.AutoSize = true;
            username_label.Location = new Point(297, 268);
            username_label.Name = "username_label";
            username_label.Size = new Size(75, 20);
            username_label.TabIndex = 0;
            username_label.Text = "Username";
            // 
            // password_label
            // 
            password_label.AutoSize = true;
            password_label.Location = new Point(297, 308);
            password_label.Name = "password_label";
            password_label.Size = new Size(70, 20);
            password_label.TabIndex = 2;
            password_label.Text = "Password";
            // 
            // username_textbox
            // 
            username_textbox.Location = new Point(378, 268);
            username_textbox.Name = "username_textbox";
            username_textbox.PlaceholderText = "Enter username";
            username_textbox.Size = new Size(125, 27);
            username_textbox.TabIndex = 1;
            // 
            // password_textbox
            // 
            password_textbox.Location = new Point(378, 308);
            password_textbox.Name = "password_textbox";
            password_textbox.PlaceholderText = "Enter password";
            password_textbox.Size = new Size(125, 27);
            password_textbox.TabIndex = 3;
            password_textbox.UseSystemPasswordChar = true;
            // 
            // remember_me_checkbox
            // 
            remember_me_checkbox.AutoSize = true;
            remember_me_checkbox.Location = new Point(346, 341);
            remember_me_checkbox.Name = "remember_me_checkbox";
            remember_me_checkbox.Size = new Size(129, 24);
            remember_me_checkbox.TabIndex = 4;
            remember_me_checkbox.Text = "Remember me";
            remember_me_checkbox.UseVisualStyleBackColor = true;
            // 
            // LoginControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SteelBlue;
            Controls.Add(password_textbox);
            Controls.Add(remember_me_checkbox);
            Controls.Add(login_button);
            Controls.Add(username_textbox);
            Controls.Add(password_label);
            Controls.Add(username_label);
            Name = "LoginControl";
            Size = new Size(800, 450);
            ResumeLayout(false);
            PerformLayout();
        }

        private Button login_button;
        private Label username_label;
        private Label password_label;
        private TextBox username_textbox;
        private TextBox password_textbox;
        private CheckBox remember_me_checkbox;

        #endregion

    }
}
