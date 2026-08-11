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
            chat_history_textbox = new TextBox();
            chat_input_textbox = new TextBox();
            send_chat_button = new Button();
            lobby_status_label = new Label();
            SuspendLayout();
            // 
            // lobby_id_textbox
            // 
            lobby_id_textbox.Location = new Point(473, 118);
            lobby_id_textbox.Name = "lobby_id_textbox";
            lobby_id_textbox.Size = new Size(246, 27);
            lobby_id_textbox.TabIndex = 0;
            // 
            // lobby_id_label
            // 
            lobby_id_label.AutoSize = true;
            lobby_id_label.BackColor = Color.Cornsilk;
            lobby_id_label.Location = new Point(473, 90);
            lobby_id_label.Name = "lobby_id_label";
            lobby_id_label.Size = new Size(107, 20);
            lobby_id_label.TabIndex = 1;
            lobby_id_label.Text = "Enter Lobby ID";
            // 
            // confirm_lobby_id_button
            // 
            confirm_lobby_id_button.Location = new Point(741, 116);
            confirm_lobby_id_button.Name = "confirm_lobby_id_button";
            confirm_lobby_id_button.Size = new Size(110, 31);
            confirm_lobby_id_button.TabIndex = 2;
            confirm_lobby_id_button.Text = "CONFIRM";
            confirm_lobby_id_button.UseVisualStyleBackColor = true;
            confirm_lobby_id_button.Click += confirm_lobby_id_button_Click;
            // 
            // chat_history_textbox
            // 
            chat_history_textbox.Location = new Point(300, 188);
            chat_history_textbox.Multiline = true;
            chat_history_textbox.Name = "chat_history_textbox";
            chat_history_textbox.ReadOnly = true;
            chat_history_textbox.ScrollBars = ScrollBars.Vertical;
            chat_history_textbox.Size = new Size(680, 360);
            chat_history_textbox.TabIndex = 3;
            // 
            // chat_input_textbox
            // 
            chat_input_textbox.Location = new Point(300, 574);
            chat_input_textbox.Name = "chat_input_textbox";
            chat_input_textbox.Size = new Size(560, 27);
            chat_input_textbox.TabIndex = 4;
            // 
            // send_chat_button
            // 
            send_chat_button.Location = new Point(880, 572);
            send_chat_button.Name = "send_chat_button";
            send_chat_button.Size = new Size(100, 31);
            send_chat_button.TabIndex = 5;
            send_chat_button.Text = "SEND";
            send_chat_button.UseVisualStyleBackColor = true;
            send_chat_button.Click += send_chat_button_Click;
            // 
            // lobby_status_label
            // 
            lobby_status_label.AutoSize = true;
            lobby_status_label.BackColor = Color.Cornsilk;
            lobby_status_label.Location = new Point(300, 160);
            lobby_status_label.Name = "lobby_status_label";
            lobby_status_label.Size = new Size(126, 20);
            lobby_status_label.TabIndex = 6;
            lobby_status_label.Text = "Not in a lobby yet";
            // 
            // FindLobbyControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.CornflowerBlue;
            Controls.Add(lobby_status_label);
            Controls.Add(send_chat_button);
            Controls.Add(chat_input_textbox);
            Controls.Add(chat_history_textbox);
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
        private TextBox chat_history_textbox;
        private TextBox chat_input_textbox;
        private Button send_chat_button;
        private Label lobby_status_label;
    }
}
