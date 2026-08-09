using System;
using System.Collections.Generic;
using System.Text;

using FXTVGame.Backend.Models;
using FXTVGame.Backend.Services;
using System.Security.Cryptography.Xml;

namespace FXTVGame.Launcher.Services.Register
{
    internal class RegisterService
    {
        public RegisterResult Register(string username_form, string password_form, string repassword_form)
        {
            string username = username_form.Trim();

            RegisterResult validationResult = ValidateRegForm(username, password_form, repassword_form);

            if (!validationResult.Success)
            {
                return validationResult;
            }

            DatabaseService databaseService = new DatabaseService();
            bool userAdded = databaseService.AddUser(username, password_form);

            if (!userAdded)
            {
                return new RegisterResult { Message = "Username already taken.", Success = false, FormSlot = 0 };
            }

            return new RegisterResult { Message = "Sign up completed.", Success = true };
        }

        public RegisterResult ValidateRegForm(string username_form, string password_form, string repassword_form )
        {
            DatabaseService databaseService = new DatabaseService();
            string username = username_form.Trim();

            if (databaseService.CheckIfUserExistsAndGetId(username).SearchResult)
            {
                return new RegisterResult { Message = "Username already taken.", Success = false, FormSlot = 0 };
            }
            if (username.Length < 3)
            {
                return new RegisterResult { Message = "Username must be at least 3 characters.", Success = false, FormSlot = 0 };
            }

            if (username.Length > 20)
            {
                return new RegisterResult { Message = "Username must be 20 characters or less.", Success = false, FormSlot = 0 };
            }

            if (username.Any(character => !char.IsLetterOrDigit(character) && character != '_'))
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
