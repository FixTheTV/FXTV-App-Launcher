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

            //RegisterResult validationResult = ValidateRegForm(username, password_form, repassword_form);

            //if (!validationResult.Success)
            //{
            //    return validationResult;
            //}

            DatabaseService databaseService = new DatabaseService();
            bool userAdded = databaseService.AddUser(username, password_form);

            if (!userAdded)
            {
                return new RegisterResult { Message = "Username already taken.", Success = false, FormSlot = 0 };
            }

            return new RegisterResult { Message = "Sign up completed.", Success = true };
        }
    }
}
