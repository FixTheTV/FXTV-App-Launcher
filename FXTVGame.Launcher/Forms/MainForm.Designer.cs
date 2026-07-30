namespace FXTVGame.Launcher.Forms
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            enter_login_button = new Button();
            SuspendLayout();
            // 
            // enter_login_button
            // 
            enter_login_button.Location = new Point(294, 248);
            enter_login_button.Name = "enter_login_button";
            enter_login_button.Size = new Size(167, 29);
            enter_login_button.TabIndex = 0;
            enter_login_button.Text = "GOTO LOGIN";
            enter_login_button.UseVisualStyleBackColor = true;
            enter_login_button.Click += btnEnterLogin_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(800, 450);
            Controls.Add(enter_login_button);
            Name = "MainForm";
            Text = "Main Form";
            ResumeLayout(false);
        }

        #endregion

        private Button enter_login_button;
    }
}