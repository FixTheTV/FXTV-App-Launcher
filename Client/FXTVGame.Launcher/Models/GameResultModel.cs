using System.Text.Json.Serialization;

namespace FXTVGame.Launcher.Models
{
    internal sealed class GameResultModel
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("game_id")]
        public int GameId { get; set; }

        [JsonPropertyName("result")]
        public string Result { get; set; } = string.Empty;
    }
}
