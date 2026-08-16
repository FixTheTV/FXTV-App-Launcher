namespace FXTVGame.Launcher.Controls
{
    partial class Lobby
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
            lobby_status_label = new Label();
            send_chat_button = new Button();
            chat_input_textbox = new TextBox();
            chat_history_textbox = new TextBox();
            SuspendLayout();
            // 
            // lobby_status_label
            // 
            lobby_status_label.AutoSize = true;
            lobby_status_label.BackColor = Color.Cornsilk;
            lobby_status_label.Location = new Point(584, 139);
            lobby_status_label.Name = "lobby_status_label";
            lobby_status_label.Size = new Size(128, 20);
            lobby_status_label.TabIndex = 10;
            lobby_status_label.Text = "Not in a lobby yet";
            // 
            // send_chat_button
            // 
            send_chat_button.Location = new Point(884, 612);
            send_chat_button.Name = "send_chat_button";
            send_chat_button.Size = new Size(100, 31);
            send_chat_button.TabIndex = 9;
            send_chat_button.Text = "SEND";
            send_chat_button.UseVisualStyleBackColor = true;
            // 
            // chat_input_textbox
            // 
            chat_input_textbox.Location = new Point(304, 614);
            chat_input_textbox.Name = "chat_input_textbox";
            chat_input_textbox.Size = new Size(560, 27);
            chat_input_textbox.TabIndex = 8;
            // 
            // chat_history_textbox
            // 
            chat_history_textbox.Location = new Point(304, 228);
            chat_history_textbox.Multiline = true;
            chat_history_textbox.Name = "chat_history_textbox";
            chat_history_textbox.ReadOnly = true;
            chat_history_textbox.ScrollBars = ScrollBars.Vertical;
            chat_history_textbox.Size = new Size(680, 360);
            chat_history_textbox.TabIndex = 7;
            // 
            // Lobby
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.AntiqueWhite;
            Controls.Add(lobby_status_label);
            Controls.Add(send_chat_button);
            Controls.Add(chat_input_textbox);
            Controls.Add(chat_history_textbox);
            Name = "Lobby";
            Size = new Size(1280, 720);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lobby_status_label;
        private Button send_chat_button;
        private TextBox chat_input_textbox;
        private TextBox chat_history_textbox;
    }
}
