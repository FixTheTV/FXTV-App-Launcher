namespace FXTVGame.Launcher.Controls
{
    partial class FindLobbyControl
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
            lobby_id_textbox = new TextBox();
            lobby_id_label = new Label();
            confirm_lobby_id_button = new Button();
            SuspendLayout();
            // 
            // lobby_id_textbox
            // 
            lobby_id_textbox.Location = new Point(457, 332);
            lobby_id_textbox.Name = "lobby_id_textbox";
            lobby_id_textbox.Size = new Size(246, 27);
            lobby_id_textbox.TabIndex = 0;
            // 
            // lobby_id_label
            // 
            lobby_id_label.AutoSize = true;
            lobby_id_label.BackColor = Color.Cornsilk;
            lobby_id_label.Location = new Point(457, 304);
            lobby_id_label.Name = "lobby_id_label";
            lobby_id_label.Size = new Size(107, 20);
            lobby_id_label.TabIndex = 1;
            lobby_id_label.Text = "Enter Lobby ID";
            // 
            // confirm_lobby_id_button
            // 
            confirm_lobby_id_button.Location = new Point(725, 330);
            confirm_lobby_id_button.Name = "confirm_lobby_id_button";
            confirm_lobby_id_button.Size = new Size(110, 31);
            confirm_lobby_id_button.TabIndex = 2;
            confirm_lobby_id_button.Text = "CONFIRM";
            confirm_lobby_id_button.UseVisualStyleBackColor = true;
            confirm_lobby_id_button.Click += this.confirm_lobby_id_button_Click;
            // 
            // FindLobbyControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.CornflowerBlue;
            Controls.Add(confirm_lobby_id_button);
            Controls.Add(lobby_id_label);
            Controls.Add(lobby_id_textbox);
            Name = "FindLobbyControl";
            Size = new Size(1280, 720);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox lobby_id_textbox;
        private Label lobby_id_label;
        private Button confirm_lobby_id_button;
    }
}
