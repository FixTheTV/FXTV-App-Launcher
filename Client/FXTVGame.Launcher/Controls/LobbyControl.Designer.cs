namespace FXTVGame.Launcher.Controls
{
    partial class LobbyControl
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
            chat_history_textbox = new TextBox();
            chat_input_textbox = new TextBox();
            send_chat_button = new Button();
            lobby_status_label = new Label();
            SuspendLayout();
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
            lobby_status_label.Size = new Size(128, 20);
            lobby_status_label.TabIndex = 6;
            lobby_status_label.Text = "Not in a lobby yet";
            // 
            // LobbyControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.CornflowerBlue;
            Controls.Add(lobby_status_label);
            Controls.Add(send_chat_button);
            Controls.Add(chat_input_textbox);
            Controls.Add(chat_history_textbox);
            Name = "LobbyControl";
            Size = new Size(1280, 720);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox chat_history_textbox;
        private TextBox chat_input_textbox;
        private Button send_chat_button;
        private Label lobby_status_label;
    }
}
