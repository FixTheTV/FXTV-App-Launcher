using FXTVGame.Launcher.Models;
using FXTVGame.Launcher.Services;

namespace FXTVGame.Launcher.Controls
{
    public partial class Lobby : UserControl
    {
        private readonly NetworkService networkService = NetworkService.Shared;

        public Lobby(int lobbyID, int userCount)
        {
            InitializeComponent();
            this.lobby_status_label.Text = $"Lobby: {lobbyID} | Player: {userCount} / 4 ";
            this.Load += Lobby_Load;
        }

        private void Lobby_Load(object? sender, EventArgs e)
        {
            networkService.LobbyChatReceived += OnLobbyChatReceived;
            networkService.StartLobbyChatReceiveLoop();
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
            if (string.IsNullOrWhiteSpace(message)) return;

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
            if (!IsHandleCreated || IsDisposed) return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnLobbyChatReceived(chatMessage)));
                return;
            }

            chat_history_textbox.AppendText($"{chatMessage.Username}: {chatMessage.Message}{Environment.NewLine}");
        }
    }
}