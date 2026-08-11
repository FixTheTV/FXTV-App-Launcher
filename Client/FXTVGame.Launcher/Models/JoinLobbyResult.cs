namespace FXTVGame.Launcher.Models
{
    public class JoinLobbyResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int LobbyId { get; set; }
        public int PlayerCount { get; set; }
    }
}
