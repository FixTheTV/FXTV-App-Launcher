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
    }
}
