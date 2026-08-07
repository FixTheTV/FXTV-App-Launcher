using FXTVGame.Launcher.Forms;

namespace FXTVGame.Launcher
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}