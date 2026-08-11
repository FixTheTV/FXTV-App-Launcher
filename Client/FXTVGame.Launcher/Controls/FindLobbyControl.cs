using FXTVGame.Launcher.Models;
using FXTVGame.Launcher.Services;

namespace FXTVGame.Launcher.Controls
{
    public partial class FindLobbyControl : UserControl
    {
        private readonly NetworkService networkService = NetworkService.Shared;

        public FindLobbyControl()
        {
            InitializeComponent();
            chat_input_textbox.Enabled = false;
            send_chat_button.Enabled = false;
            networkService.LobbyChatReceived += OnLobbyChatReceived;
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            networkService.LobbyChatReceived -= OnLobbyChatReceived;
            networkService.StopLobbyChatReceiveLoop();
            base.OnHandleDestroyed(e);
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

                lobby_status_label.Text = result.Message;
                chat_input_textbox.Enabled = true;
                send_chat_button.Enabled = true;
                networkService.StartLobbyChatReceiveLoop();
            }
            catch
            {
                MessageBox.Show("Failed to join lobby.");
            }
        }

        private async void send_chat_button_Click(object sender, EventArgs e)
        {
            string message = chat_input_textbox.Text.Trim();

            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            try
            {
                await networkService.SendLobbyChatPacket(message);
                chat_input_textbox.Clear();
            }
            catch
            {
                MessageBox.Show("Failed to send message.");
            }
        }

        private void OnLobbyChatReceived(LobbyChatMessage chatMessage)
        {
            if (InvokeRequired)
            {
                BeginInvoke(() => OnLobbyChatReceived(chatMessage));
                return;
            }

            chat_history_textbox.AppendText($"{chatMessage.Username}: {chatMessage.Message}{Environment.NewLine}");
        }
    }
}
