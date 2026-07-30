using FXTVGame.Launcher.Models.Auth;
using FXTVGame.Launcher.Services.Database;

namespace FXTVGame.Launcher.Services.Auth
{
    public class AuthService
    {
        private readonly DatabaseService databaseService = new DatabaseService();

        public AuthService()
        {
            databaseService.Initialize();
            databaseService.AddUser("admin", "123");
        }

        public AuthResult Login(string username, string password)
        {
            bool userExists = databaseService.UserExists(username, password);

            if (userExists)
            {
                return new AuthResult
                {
                    Success = true,
                    Message = "Login successful."
                };
            }

            return new AuthResult
            {
                Success = false,
                Message = "Incorrect username or password."
            };
        }
    }
}