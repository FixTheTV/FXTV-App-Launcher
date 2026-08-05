using FXTVGame.Launcher.Models.Auth;
using FXTVGame.Launcher.Services.Database;

namespace FXTVGame.Launcher.Services.Auth
{
    public class AuthService
    {
        private readonly DatabaseService databaseService = new DatabaseService();

        public AuthService()
        {
        }

        public AuthResult Login(string username, string password)
        {
            username = username.Trim();

            var checkUserExists = databaseService.CheckIfUserExistsAndGetId(username);

            if (checkUserExists.SearchResult)
            {
                if (databaseService.CheckPassword(checkUserExists.UserId, password))
                {
                    return new AuthResult
                    {
                        Success = true,
                        Message = "Login successful.",
                        UserId = checkUserExists.UserId,
                        Username = username
                    };
                }
                return new AuthResult
                {
                    Success = false,
                    Message = "Incorrect username or password."
                };
                
            }
            else
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "You don't have an existing account."
                };
            }
                      
        }
    }
}
