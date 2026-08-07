namespace FXTVGame.Launcher.Controls
{
    partial class MainControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            register_button = new Button();
            SuspendLayout();
            // 
            // login_button
            // 
            login_button.Location = new Point(93, 156);
            login_button.Name = "login_button";
            login_button.Size = new Size(142, 36);
            login_button.TabIndex = 1;
            login_button.Text = "Login";
            login_button.UseVisualStyleBackColor = true;
            login_button.Click += btnGoToLogin_Click;
            // 
            // register_button
            // 
            register_button.Location = new Point(93, 208);
            register_button.Name = "register_button";
            register_button.Size = new Size(142, 36);
            register_button.TabIndex = 1;
            register_button.Text = "Sign Up";
            register_button.UseVisualStyleBackColor = true;
            register_button.Click += btnGoToRegister_Click;
            // 
            // MainControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            Controls.Add(login_button);
            Controls.Add(register_button);
            Name = "MainControl";
            Size = new Size(328, 360);
            ResumeLayout(false);
        }

        #endregion

        private Button login_button;
        private Button register_button;
    }
}
