using System;
using System.Collections.Generic;
using System.Text;

using FXTVGame.Launcher.Models.Register;
using FXTVGame.Launcher.Services;
using System.Security.Cryptography.Xml;
using System.Net.Sockets;

namespace FXTVGame.Launcher.Services
{
    internal class RegisterService
    {
        private readonly NetworkService network = NetworkService.Shared;
        public async Task<RegisterResult> ValidateRegForm(string username_form, string password_form, string repassword_form)
        {
            if (username_form.Length < 3)
            {
                return new RegisterResult { Message = "Username must be at least 3 characters.", Success = false, FormSlot = 0 };
            }

            if (username_form.Length > 20)
            {
                return new RegisterResult { Message = "Username must be 20 characters or less.", Success = false, FormSlot = 0 };
            }

            if (username_form.Any(character => !char.IsLetterOrDigit(character) && character != '_'))
            {
                return new RegisterResult { Message = "Username can only use letters, numbers, and underscores.", Success = false, FormSlot = 0 };
            }

            if (password_form.Length < 6)
            {
                return new RegisterResult { Message = "Password must be at least 6 characters.", Success = false, FormSlot = 1 };
            }
            if (password_form != repassword_form)
            {
                return new RegisterResult { Message = "Retype password is incorrect.", Success = false, FormSlot = 2 };
            }

            await ConnectBoisss();

            await network.SendRegisterPacket(username_form, password_form);

            return await network.RecieveRegisterResultPacket();
        }
        public async Task ConnectBoisss()
        {
            await network.ConnectAsync("192.168.1.5", 12345);
        }

    }
}
