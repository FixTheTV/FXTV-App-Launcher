using FXTVGame.Launcher.Models;
using FXTVGame.Launcher.Services;

namespace FXTVGame.Launcher.Controls
{
    public partial class LobbyControl : UserControl
    {
        private readonly NetworkService networkService = NetworkService.Shared;

        private int MyLobby;
        private bool isReady;

        public Action? BackToHome;

        public LobbyControl(JoinLobbyResult res)
        {
            MyLobby = res.LobbyId;
            InitializeComponent();

            networkService.LobbyChatReceived += OnLobbyChatReceived;
            networkService.UpdateLobbyCount += UpdateLobbyCount;
            networkService.UpdateLobbyTab += UpdateLobbyTab;

            this.lobby_status_label.Text = $"Server {MyLobby} | Player Count {res.PlayerCount} / 2";
            SetPlayerSlot(0, string.Empty, false);
            SetPlayerSlot(1, string.Empty, false);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            networkService.LobbyChatReceived -= OnLobbyChatReceived;
            networkService.UpdateLobbyCount -= UpdateLobbyCount;
            networkService.UpdateLobbyTab -= UpdateLobbyTab;
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

        private void UpdateLobbyTab(LobbyTab lobbyTab)
        {
            if (InvokeRequired)
            {
                BeginInvoke(() => UpdateLobbyTab(lobbyTab));
                return;
            }

            SetPlayerSlot(lobbyTab.slot, lobbyTab.name, lobbyTab.isReady);
        }

        private void SetPlayerSlot(int slot, string name, bool slotIsReady)
        {
            bool hasPlayer = !string.IsNullOrWhiteSpace(name);
            string displayName = hasPlayer ? name : "Empty";
            string playerState = hasPlayer ? (slotIsReady ? "Ready" : "Not Ready") : "Waiting";

            switch (slot)
            {
                case 0:
                    this.player_name_label1.Text = displayName;
                    this.player_state_label1.Text = playerState;
                    break;
                case 1:
                    this.player_name_label2.Text = displayName;
                    this.player_state_label2.Text = playerState;
                    break;

            }
        }

        private async void leave_lobby_button_Click(object sender, EventArgs e)
        {
            try {
                await networkService.SendLeaveLobbyPacket();
                networkService.StopLobbyChatReceiveLoop();
                BackToHome?.Invoke();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            

        }

        private async void ready_button_Click(object sender, EventArgs e)
        {
            try
            {
                isReady = !isReady;
                ready_button.Text = isReady ? "UNREADY" : "READY";
                await networkService.SendLobbyReadyPacket(isReady);
            }
            catch
            {
                isReady = !isReady;
                ready_button.Text = isReady ? "UNREADY" : "READY";
                MessageBox.Show("Failed to update ready state.");
            }
        }
    }
}
