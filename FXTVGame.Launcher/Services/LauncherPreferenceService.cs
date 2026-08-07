using FXTVGame.Launcher.Models;

namespace FXTVGame.Launcher.Services
{
    public class LauncherPreferenceService
    {
        private const string DefaultResolution = "1280 x 720";
        private const string DefaultWindowMode = "Windowed";

        private static readonly string[] ValidWindowModes =
        {
            "Windowed",
            "Borderless Windowed",
            "Fullscreen"
        };

        private readonly string filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "FXTVGame",
            "Launcher",
            "display-settings.txt"
        );

        public LauncherDisplaySettings LoadSettings()
        {
            string resolution = DefaultResolution;
            string windowMode = DefaultWindowMode;

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                if (lines.Length > 0 && TryGetSize(lines[0], out _))
                {
                    resolution = lines[0];
                }

                if (lines.Length > 1 && IsValidWindowMode(lines[1]))
                {
                    windowMode = lines[1];
                }
            }

            return new LauncherDisplaySettings
            {
                Resolution = resolution,
                WindowMode = windowMode,
                ScreenSize = GetSizeOrDefault(resolution)
            };
        }

        public string LoadResolution()
        {
            return LoadSettings().Resolution;
        }

        public Size LoadScreenSize()
        {
            return LoadSettings().ScreenSize;
        }

        public void SaveResolution(string resolution)
        {
            SaveSettings(resolution, LoadSettings().WindowMode);
        }

        public void SaveSettings(string resolution, string windowMode)
        {
            if (!TryGetSize(resolution, out _))
            {
                resolution = DefaultResolution;
            }

            if (!IsValidWindowMode(windowMode))
            {
                windowMode = DefaultWindowMode;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            File.WriteAllLines(filePath, new[] { resolution, windowMode });
        }

        private Size GetSizeOrDefault(string resolution)
        {
            if (TryGetSize(resolution, out Size size))
            {
                return size;
            }

            return new Size(1280, 720);
        }

        private bool IsValidWindowMode(string windowMode)
        {
            return ValidWindowModes.Contains(windowMode);
        }

        private bool TryGetSize(string resolution, out Size size)
        {
            size = Size.Empty;

            string[] parts = resolution.Split('x', StringSplitOptions.TrimEntries);

            if (parts.Length != 2)
            {
                return false;
            }

            if (!int.TryParse(parts[0], out int width))
            {
                return false;
            }

            if (!int.TryParse(parts[1], out int height))
            {
                return false;
            }

            size = new Size(width, height);
            return true;
        }
    }
}
