using System;
using System.Collections.Generic;
using System.Text;

using FXTVGame.Launcher.Models.Register;
using FXTVGame.Launcher.Services.Database;
using System.Security.Cryptography.Xml;

namespace FXTVGame.Launcher.Services.Register
{
    internal class RegisterService
    {
        public RegisterResult ValidateRegForm(string username_form, string password_form, string repassword_form )
        {
            DatabaseService databaseService = new DatabaseService();

            if (databaseService.CheckIfUserExistsAndGetId(username_form).SearchResult)
            {
                return new RegisterResult { Message = "Username already taken.", Success = false, FormSlot = 0 };
            }
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
                return new RegisterResult { Message = "Username can only use letters, numbers, and underscores.", Success = false , FormSlot = 0};
            }

            if (password_form.Length < 6)
            {
                return new RegisterResult { Message = "Password must be at least 6 characters.", Success = false, FormSlot = 1 };
            }
            if (password_form != repassword_form)
            {
                return new RegisterResult { Message = "Retype password is incorrect.", Success = false, FormSlot = 2 };
            }

            return new RegisterResult { Message = "Sign up completed.", Success = true };
        }
    }
}
