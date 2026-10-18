using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FXTVGame.Launcher.Services
{
    internal class GameService
    {
        private readonly string txtGameExePath = @"C:\Users\ADMIN\source\repos\FXTVGame\Client\FXTVGame.Game\TicTacToeGame.exe";
        private HTTPService? _httpService;

        public void StartGame(int gameId, string sessionToken, string serverIp)
        {
            _httpService = new HTTPService();
            _httpService.MatchFinishedAsync = async result =>
            {
                if (result.GameId != gameId)
                {
                    throw new InvalidDataException($"Game result ID {result.GameId} does not match launched game {gameId}.");
                }

                await NetworkService.Shared.SendGameFinishedPacketAsync(result);
            };

            var gamePath = txtGameExePath;
            var token = sessionToken.Trim();

            var startInfo = new ProcessStartInfo
            {
                FileName = gamePath,
                UseShellExecute = false
            };

            startInfo.ArgumentList.Add("--");
            startInfo.ArgumentList.Add("--game");
            startInfo.ArgumentList.Add(gameId.ToString());
            startInfo.ArgumentList.Add("--token");
            startInfo.ArgumentList.Add(token);
            startInfo.ArgumentList.Add("--server");
            startInfo.ArgumentList.Add(serverIp);
            startInfo.ArgumentList.Add("--localport");
            startInfo.ArgumentList.Add((_httpService.getLocalPort()).ToString());
            startInfo.ArgumentList.Add("--callback-url");
            startInfo.ArgumentList.Add($"http://127.0.0.1:{_httpService.getLocalPort()}/match-end");

            Process.Start(startInfo);

        }

    }
}


