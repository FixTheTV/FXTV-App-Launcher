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
            offline_play_button = new Button();
            online_play_button = new Button();
            settings_button = new Button();
            SuspendLayout();
            // 
            // welcome_label
            // 
            welcome_label.AutoSize = true;
            welcome_label.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            welcome_label.Location = new Point(80, 64);
            welcome_label.Name = "welcome_label";
            welcome_label.Size = new Size(0, 41);
            welcome_label.TabIndex = 0;
            // 
            // logout_button
            // 
            logout_button.Location = new Point(1070, 624);
            logout_button.Name = "logout_button";
            logout_button.Size = new Size(142, 36);
            logout_button.TabIndex = 1;
            logout_button.Text = "LOG OUT";
            logout_button.UseVisualStyleBackColor = true;
            logout_button.Click += logout_button_Click;
            // 
            // offline_play_button
            // 
            offline_play_button.Location = new Point(515, 250);
            offline_play_button.Name = "offline_play_button";
            offline_play_button.Size = new Size(225, 75);
            offline_play_button.TabIndex = 2;
            offline_play_button.Text = "PLAY OFFLINE";
            offline_play_button.UseVisualStyleBackColor = true;
            // 
            // online_play_button
            // 
            online_play_button.Location = new Point(515, 346);
            online_play_button.Name = "online_play_button";
            online_play_button.Size = new Size(225, 75);
            online_play_button.TabIndex = 2;
            online_play_button.Text = "PLAY ONLINE";
            online_play_button.UseVisualStyleBackColor = true;
            // 
            // settings_button
            // 
            settings_button.Location = new Point(515, 442);
            settings_button.Name = "settings_button";
            settings_button.Size = new Size(225, 75);
            settings_button.TabIndex = 2;
            settings_button.Text = "SETTINGS";
            settings_button.UseVisualStyleBackColor = true;
            settings_button.Click += settings_button_Click;
            // 
            // HomeControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            Controls.Add(settings_button);
            Controls.Add(online_play_button);
            Controls.Add(offline_play_button);
            Controls.Add(logout_button);
            Controls.Add(welcome_label);
            Name = "HomeControl";
            Size = new Size(1270, 820);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label welcome_label;
        private Button logout_button;
        private Button offline_play_button;
        private Button online_play_button;
        private Button settings_button;
    }
}
