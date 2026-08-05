using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FXTVGame.Launcher.Services;

namespace FXTVGame.Launcher.Controls
{
    public partial class SettingControl : UserControl
    {
        private readonly LauncherPreferenceService preferenceService = new LauncherPreferenceService();

        public event Action<Size>? SettingApplied;
        public event Action? BackToHome;
        public SettingControl()
        {
            InitializeComponent();
            LoadPreferences();
        }

        private void back_button_Click(object sender, EventArgs e)
        {
            BackToHome?.Invoke();
        }

        private void apply_button_Click(object sender, EventArgs e)
        {
            string resolution = resolution_combo_box.Text;

            preferenceService.SaveResolution(resolution);
            Size newSize = preferenceService.LoadScreenSize();
            Size = newSize;
            CenterContent();
            SettingApplied?.Invoke(newSize);
        }

        private void LoadPreferences()
        {
            string resolution = preferenceService.LoadResolution();

            if (!resolution_combo_box.Items.Contains(resolution))
            {
                resolution_combo_box.Items.Insert(0, resolution);
            }

            resolution_combo_box.Text = resolution;
            window_mode_combo_box.SelectedIndex = 0;
            Size = preferenceService.LoadScreenSize();
            CenterContent();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            CenterContent();
        }

        private void CenterContent()
        {
            if (display_group_box == null)
            {
                return;
            }

            int contentWidth = display_group_box.Width;
            int contentX = (Width - contentWidth) / 2;
            int titleY = Math.Max(48, Height / 2 - 300);

            settings_title_label.Location = new Point(contentX, titleY);
            settings_subtitle_label.Location = new Point(contentX + 6, settings_title_label.Bottom + 4);
            display_group_box.Location = new Point(contentX, settings_subtitle_label.Bottom + 40);

            back_button.Location = new Point(contentX, display_group_box.Bottom + 36);
            apply_button.Location = new Point(contentX + contentWidth - apply_button.Width, display_group_box.Bottom + 36);
            note_label.Location = new Point(contentX, back_button.Bottom + 36);
        }
    }
}
