using System;
using System.Collections.Generic;
using System.Text;

namespace FXTVGame.Backend.Network
{
    public static class Opcodes
    {
        public const ushort C2S_Login = 1001;
        public const ushort S2C_LoginResult = 2001;

        public const ushort C2S_Register = 1002;
        public const ushort S2C_RegisterResult = 2002;

        public const ushort C2S_Logout = 1003;
        public const ushort S2C_LogoutResult = 2003;

        public const ushort C2S_LeaveLobby = 1104;
        public const ushort S2C_LeaveLobby = 2104;
        public const ushort S2C_LobbyTabUpdate = 2105;

        public const ushort C2S_JoinLobby = 1101;
        public const ushort S2C_JoinLobbyResult = 2101;

        public const ushort C2S_LobbyChat = 1102;
        public const ushort S2C_LobbyChat = 2102;

        public const ushort C2S_LobbyReady = 1103;

        public const ushort S2C_UpdateLobbyCount = 2103;
        public const ushort S2C_Ping = 2000;
        public const ushort C2S_Pong = 1000;
    }
}
