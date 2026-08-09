using System;
using System.Collections.Generic;
using System.Text;

namespace FXTVGame.Backend.Models
{
    internal class RegisterResult
    {
        public string Message { get; set; } = " ";

        public bool Success { get; set; }

        public int FormSlot { get; set; }
        
    }
}
