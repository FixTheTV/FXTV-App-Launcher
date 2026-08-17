using FXTVGame.Launcher.Models;
using FXTVGame.Launcher.Services;

namespace FXTVGame.Launcher.Controls
{
    public partial class FindLobbyControl : UserControl
    {
        private readonly NetworkService networkService = NetworkService.Shared;

        public event Action<JoinLobbyResult>? GoToLobby;

        public FindLobbyControl()
        {
            InitializeComponent();
        }


        private async void confirm_lobby_id_button_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(lobby_id_textbox.Text, out int lobbyId))
            {
                MessageBox.Show("Lobby ID must be a number.");
                return;
            }

            try
            {
                await networkService.SendJoinLobbyPacket(lobbyId);
                JoinLobbyResult result = await networkService.ReceiveJoinLobbyResultPacket();

                if (!result.Success)
                {
                    MessageBox.Show(result.Message);
                    return;
                }

                GoToLobby?.Invoke(result);
                networkService.StartLobbyChatReceiveLoop();
            }
            catch
            {
                MessageBox.Show("Failed to join lobby.");
            }
        }

    }
}
