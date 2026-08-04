namespace FXTVGame.Launcher.Controls
{
    partial class LogoutConfirmationDialog
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

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            logout_confirm_button = new Button();
            logout_cancel_button = new Button();
            logout_confirm_label = new Label();
            SuspendLayout();
            // 
            // logout_confirm_button
            // 
            logout_confirm_button.BackColor = SystemColors.ControlLightLight;
            logout_confirm_button.Location = new Point(30, 66);
            logout_confirm_button.Name = "logout_confirm_button";
            logout_confirm_button.Size = new Size(103, 29);
            logout_confirm_button.TabIndex = 0;
            logout_confirm_button.Text = "Log out";
            logout_confirm_button.UseVisualStyleBackColor = false;
            logout_confirm_button.Click += logout_confirm_button_Click;
            // 
            // logout_cancel_button
            // 
            logout_cancel_button.Location = new Point(167, 66);
            logout_cancel_button.Name = "logout_cancel_button";
            logout_cancel_button.Size = new Size(103, 29);
            logout_cancel_button.TabIndex = 1;
            logout_cancel_button.Text = "Cancel";
            logout_cancel_button.UseVisualStyleBackColor = true;
            logout_cancel_button.Click += logout_cancel_button_Click;
            // 
            // logout_confirm_label
            // 
            logout_confirm_label.AutoSize = true;
            logout_confirm_label.BackColor = SystemColors.ControlLight;
            logout_confirm_label.Location = new Point(30, 25);
            logout_confirm_label.Name = "logout_confirm_label";
            logout_confirm_label.Size = new Size(232, 20);
            logout_confirm_label.TabIndex = 2;
            logout_confirm_label.Text = "Are you sure you want to log out?";
            // 
            // LogoutConfirmationDialog
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.CadetBlue;
            Controls.Add(logout_confirm_label);
            Controls.Add(logout_cancel_button);
            Controls.Add(logout_confirm_button);
            Name = "LogoutConfirmationDialog";
            Size = new Size(300, 120);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button logout_confirm_button;
        private Button logout_cancel_button;
        private Label logout_confirm_label;
    }
}
