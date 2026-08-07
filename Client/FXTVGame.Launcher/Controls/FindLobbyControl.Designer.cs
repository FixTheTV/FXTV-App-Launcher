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

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lobby_id_textbox = new TextBox();
            label1 = new Label();
            confirm_lobby_id_button = new Button();
            SuspendLayout();
            // 
            // lobby_id_textbox
            // 
            lobby_id_textbox.Location = new Point(498, 358);
            lobby_id_textbox.Name = "lobby_id_textbox";
            lobby_id_textbox.Size = new Size(246, 27);
            lobby_id_textbox.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Cornsilk;
            label1.Location = new Point(566, 335);
            label1.Name = "label1";
            label1.Size = new Size(107, 20);
            label1.TabIndex = 1;
            label1.Text = "Enter Lobby ID";
            // 
            // confirm_lobby_id_button
            // 
            confirm_lobby_id_button.Location = new Point(579, 391);
            confirm_lobby_id_button.Name = "confirm_lobby_id_button";
            confirm_lobby_id_button.Size = new Size(94, 29);
            confirm_lobby_id_button.TabIndex = 2;
            confirm_lobby_id_button.Text = "CONFIRM";
            confirm_lobby_id_button.UseVisualStyleBackColor = true;
            // 
            // FindLobbyControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.CornflowerBlue;
            Controls.Add(confirm_lobby_id_button);
            Controls.Add(label1);
            Controls.Add(lobby_id_textbox);
            Name = "FindLobbyControl";
            Size = new Size(1280, 720);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox lobby_id_textbox;
        private Label label1;
        private Button confirm_lobby_id_button;
    }
}
