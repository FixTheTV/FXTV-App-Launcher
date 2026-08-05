namespace FXTVGame.Launcher.Controls
{
    partial class SettingControl
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
            settings_title_label = new Label();
            settings_subtitle_label = new Label();
            display_group_box = new GroupBox();
            window_mode_combo_box = new ComboBox();
            window_mode_label = new Label();
            resolution_combo_box = new ComboBox();
            resolution_label = new Label();
            apply_button = new Button();
            back_button = new Button();
            note_label = new Label();
            display_group_box.SuspendLayout();
            SuspendLayout();
            // 
            // settings_title_label
            // 
            settings_title_label.AutoSize = true;
            settings_title_label.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            settings_title_label.Location = new Point(72, 52);
            settings_title_label.Name = "settings_title_label";
            settings_title_label.Size = new Size(177, 54);
            settings_title_label.TabIndex = 0;
            settings_title_label.Text = "Settings";
            // 
            // settings_subtitle_label
            // 
            settings_subtitle_label.AutoSize = true;
            settings_subtitle_label.Location = new Point(78, 111);
            settings_subtitle_label.Name = "settings_subtitle_label";
            settings_subtitle_label.Size = new Size(271, 20);
            settings_subtitle_label.TabIndex = 1;
            settings_subtitle_label.Text = "Launcher and game display preferences";
            // 
            // display_group_box
            // 
            display_group_box.Controls.Add(window_mode_combo_box);
            display_group_box.Controls.Add(window_mode_label);
            display_group_box.Controls.Add(resolution_combo_box);
            display_group_box.Controls.Add(resolution_label);
            display_group_box.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            display_group_box.Location = new Point(80, 176);
            display_group_box.Name = "display_group_box";
            display_group_box.Size = new Size(520, 220);
            display_group_box.TabIndex = 2;
            display_group_box.TabStop = false;
            display_group_box.Text = "Display";
            // 
            // window_mode_combo_box
            // 
            window_mode_combo_box.DropDownStyle = ComboBoxStyle.DropDownList;
            window_mode_combo_box.Font = new Font("Segoe UI", 9F);
            window_mode_combo_box.FormattingEnabled = true;
            window_mode_combo_box.Items.AddRange(new object[] { "Windowed", "Borderless Windowed", "Fullscreen" });
            window_mode_combo_box.Location = new Point(190, 117);
            window_mode_combo_box.Name = "window_mode_combo_box";
            window_mode_combo_box.Size = new Size(260, 28);
            window_mode_combo_box.TabIndex = 3;
            // 
            // window_mode_label
            // 
            window_mode_label.AutoSize = true;
            window_mode_label.Font = new Font("Segoe UI", 9F);
            window_mode_label.Location = new Point(42, 120);
            window_mode_label.Name = "window_mode_label";
            window_mode_label.Size = new Size(107, 20);
            window_mode_label.TabIndex = 2;
            window_mode_label.Text = "Window mode";
            // 
            // resolution_combo_box
            // 
            resolution_combo_box.DropDownStyle = ComboBoxStyle.DropDownList;
            resolution_combo_box.Font = new Font("Segoe UI", 9F);
            resolution_combo_box.FormattingEnabled = true;
            resolution_combo_box.Items.AddRange(new object[] { "1270 x 820", "1280 x 720", "1600 x 900", "1920 x 1080" });
            resolution_combo_box.Location = new Point(190, 61);
            resolution_combo_box.Name = "resolution_combo_box";
            resolution_combo_box.Size = new Size(260, 28);
            resolution_combo_box.TabIndex = 1;
            // 
            // resolution_label
            // 
            resolution_label.AutoSize = true;
            resolution_label.Font = new Font("Segoe UI", 9F);
            resolution_label.Location = new Point(42, 64);
            resolution_label.Name = "resolution_label";
            resolution_label.Size = new Size(79, 20);
            resolution_label.TabIndex = 0;
            resolution_label.Text = "Resolution";
            // 
            // apply_button
            // 
            apply_button.Location = new Point(380, 432);
            apply_button.Name = "apply_button";
            apply_button.Size = new Size(220, 44);
            apply_button.TabIndex = 3;
            apply_button.Text = "APPLY";
            apply_button.UseVisualStyleBackColor = true;
            apply_button.Click += apply_button_Click;
            // 
            // back_button
            // 
            back_button.Location = new Point(80, 432);
            back_button.Name = "back_button";
            back_button.Size = new Size(220, 44);
            back_button.TabIndex = 4;
            back_button.Text = "BACK";
            back_button.UseVisualStyleBackColor = true;
            back_button.Click += back_button_Click;
            // 
            // note_label
            // 
            note_label.AutoSize = true;
            note_label.Location = new Point(80, 515);
            note_label.Name = "note_label";
            note_label.Size = new Size(448, 20);
            note_label.TabIndex = 5;
            note_label.Text = "Saved display settings can be passed to the game when launching.";
            // 
            // SettingControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            Controls.Add(note_label);
            Controls.Add(back_button);
            Controls.Add(apply_button);
            Controls.Add(display_group_box);
            Controls.Add(settings_subtitle_label);
            Controls.Add(settings_title_label);
            ForeColor = SystemColors.ControlText;
            Name = "SettingControl";
            Size = new Size(1270, 820);
            display_group_box.ResumeLayout(false);
            display_group_box.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label settings_title_label;
        private Label settings_subtitle_label;
        private GroupBox display_group_box;
        private ComboBox resolution_combo_box;
        private Label resolution_label;
        private ComboBox window_mode_combo_box;
        private Label window_mode_label;
        private Button apply_button;
        private Button back_button;
        private Label note_label;
    }
}
