namespace FXTVGame.Launcher.Services
{
    public class LauncherPreferenceService
    {
        private const string DefaultResolution = "1270 x 820";

        private readonly string filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "FXTVGame",
            "Launcher",
            "display-resolution.txt"
        );

        public string LoadResolution()
        {
            if (!File.Exists(filePath))
            {
                return DefaultResolution;
            }

            string resolution = File.ReadAllText(filePath);

            if (TryGetSize(resolution, out _))
            {
                return resolution;
            }

            return DefaultResolution;
        }

        public Size LoadScreenSize()
        {
            string resolution = LoadResolution();

            if (TryGetSize(resolution, out Size size))
            {
                return size;
            }

            return new Size(1270, 820);
        }

        public void SaveResolution(string resolution)
        {
            if (!TryGetSize(resolution, out _))
            {
                return;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            File.WriteAllText(filePath, resolution);
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
