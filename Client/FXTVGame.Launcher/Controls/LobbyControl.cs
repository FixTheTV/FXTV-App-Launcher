using FXTVGame.Launcher.Models;
using FXTVGame.Launcher.Services;

namespace FXTVGame.Launcher.Controls
{
    public partial class LobbyControl : UserControl
    {
        private readonly NetworkService networkService = NetworkService.Shared;

        private int MyLobby;

        public LobbyControl(JoinLobbyResult res)
        {
            MyLobby = res.LobbyId;
            InitializeComponent();
            networkService.LobbyChatReceived += OnLobbyChatReceived;
            networkService.UpdateLobbyCount += UpdateLobbyCount;
            this.lobby_status_label.Text = $"Server {MyLobby} | Player Count {res.PlayerCount} / 2";
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            networkService.LobbyChatReceived -= OnLobbyChatReceived;
            networkService.StopLobbyChatReceiveLoop();
            base.OnHandleDestroyed(e);
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

        private void UpdateLobbyCount(int count)
        {
            if (InvokeRequired)
            {
                BeginInvoke(() => UpdateLobbyCount(count));
                return;
            }

            this.lobby_status_label.Text = $"Server {MyLobby} | Player Count {count} / 2";
        }
    }
}
