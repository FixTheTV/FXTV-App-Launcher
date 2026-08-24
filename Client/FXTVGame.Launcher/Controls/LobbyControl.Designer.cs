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
            leave_lobby_button = new Button();
            ready_button = new Button();
            panel1 = new Panel();
            player_state_label2 = new Label();
            player_state_label1 = new Label();
            player_name_label2 = new Label();
            player_name_label1 = new Label();
            panel1.SuspendLayout();
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
            lobby_status_label.Location = new Point(3, 165);
            lobby_status_label.Name = "lobby_status_label";
            lobby_status_label.Size = new Size(128, 20);
            lobby_status_label.TabIndex = 6;
            lobby_status_label.Text = "Not in a lobby yet";
            // 
            // leave_lobby_button
            // 
            leave_lobby_button.Location = new Point(1100, 688);
            leave_lobby_button.Name = "leave_lobby_button";
            leave_lobby_button.Size = new Size(177, 29);
            leave_lobby_button.TabIndex = 7;
            leave_lobby_button.Text = "LEAVE";
            leave_lobby_button.UseVisualStyleBackColor = true;
            leave_lobby_button.Click += leave_lobby_button_Click;
            // 
            // ready_button
            // 
            ready_button.Location = new Point(300, 633);
            ready_button.Name = "ready_button";
            ready_button.Size = new Size(140, 40);
            ready_button.TabIndex = 8;
            ready_button.Text = "READY";
            ready_button.UseVisualStyleBackColor = true;
            ready_button.Click += ready_button_Click;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlLightLight;
            panel1.Controls.Add(player_state_label2);
            panel1.Controls.Add(player_state_label1);
            panel1.Controls.Add(player_name_label2);
            panel1.Controls.Add(player_name_label1);
            panel1.Location = new Point(3, 188);
            panel1.Name = "panel1";
            panel1.Size = new Size(260, 413);
            panel1.TabIndex = 9;
            // 
            // player_state_label2
            // 
            player_state_label2.AutoSize = true;
            player_state_label2.Location = new Point(171, 121);
            player_state_label2.Name = "player_state_label2";
            player_state_label2.Size = new Size(0, 20);
            player_state_label2.TabIndex = 3;
            // 
            // player_state_label1
            // 
            player_state_label1.AutoSize = true;
            player_state_label1.Location = new Point(171, 34);
            player_state_label1.Name = "player_state_label1";
            player_state_label1.Size = new Size(0, 20);
            player_state_label1.TabIndex = 2;
            // 
            // player_name_lable2
            // 
            player_name_label2.AutoSize = true;
            player_name_label2.Location = new Point(16, 121);
            player_name_label2.Name = "player_name_lable2";
            player_name_label2.Size = new Size(0, 20);
            player_name_label2.TabIndex = 1;
            // 
            // player_name_label1
            // 
            player_name_label1.AutoSize = true;
            player_name_label1.Location = new Point(16, 34);
            player_name_label1.Name = "player_name_label1";
            player_name_label1.Size = new Size(0, 20);
            player_name_label1.TabIndex = 0;
            // 
            // LobbyControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.CornflowerBlue;
            Controls.Add(ready_button);
            Controls.Add(panel1);
            Controls.Add(leave_lobby_button);
            Controls.Add(lobby_status_label);
            Controls.Add(send_chat_button);
            Controls.Add(chat_input_textbox);
            Controls.Add(chat_history_textbox);
            Name = "LobbyControl";
            Size = new Size(1280, 720);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox chat_history_textbox;
        private TextBox chat_input_textbox;
        private Button send_chat_button;
        private Label lobby_status_label;
        private Button leave_lobby_button;
        private Button ready_button;
        private Panel panel1;
        private Label player_name_label1;
        private Label player_state_label2;
        private Label player_state_label1;
        private Label player_name_label2;
    }
}
