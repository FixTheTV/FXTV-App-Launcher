namespace FXTVGame.Launcher.Controls
{
    partial class HomeControl
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
            welcome_label = new Label();
            logout_button = new Button();
            SuspendLayout();
            // 
            // welcome_label
            // 
            welcome_label.AutoSize = true;
            welcome_label.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            welcome_label.Location = new Point(147, 87);
            welcome_label.Name = "welcome_label";
            welcome_label.Size = new Size(206, 41);
            welcome_label.TabIndex = 0;
            welcome_label.Text = "Hello user";
            // 
            // logout_button
            // 
            logout_button.Location = new Point(179, 157);
            logout_button.Name = "logout_button";
            logout_button.Size = new Size(142, 36);
            logout_button.TabIndex = 1;
            logout_button.Text = "LOG OUT";
            logout_button.UseVisualStyleBackColor = true;
            logout_button.Click += logout_button_Click;
            // 
            // HomeControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            Controls.Add(logout_button);
            Controls.Add(welcome_label);
            Name = "HomeControl";
            Size = new Size(500, 300);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label welcome_label;
        private Button logout_button;
    }
}
