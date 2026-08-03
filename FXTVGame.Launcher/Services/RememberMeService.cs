namespace FXTVGame.Launcher.Services
{
    public class RememberMeService
    {
        private readonly string filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "FXTVGame",
            "Launcher",
            "remembered-user.txt"
        );

        public string LoadUsername()
        {
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }

            return File.ReadAllText(filePath);
        }

        public void SaveUsername(string username)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            File.WriteAllText(filePath, username);
        }

        public void ClearUsername()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
