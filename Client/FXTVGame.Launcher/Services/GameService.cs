using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FXTVGame.Launcher.Services
{
    internal class GameService
    {
        private readonly string txtGameExePath = @"C:\Users\ADMIN\source\repos\FXTVGame\Client\FXTVGame.Game\TicTacToeGame.exe";
        
        public void StartGame(int gameId, string sessionToken, string serverIp)
        {
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

            Process.Start(startInfo);

        }
    }
}


