namespace FXTVGame.Launcher.Models
{
    public class LauncherDisplaySettings
    {
        public string Resolution { get; set; } = "1280 x 720";
        public string WindowMode { get; set; } = "Windowed";
        public Size ScreenSize { get; set; } = new Size(1280, 720);
    }
}
