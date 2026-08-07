using System;
using System.Collections.Generic;
using System.Text;

namespace FXTVGame.Launcher.Models.Auth
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public long UserId { get; set; }

        public string Username { get; set; } = string.Empty;
    }
}
