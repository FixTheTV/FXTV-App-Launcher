using FXTVGame.Launcher.Models.Database;
using Microsoft.Data.Sqlite;

namespace FXTVGame.Launcher.Services.Database
{
    public class DatabaseService
    {
        private readonly string connectionString = "Data Source=UserAuth.db;";

        public DatabaseService()
        {
            Initialize();
        }

        public void Initialize()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var createTableCmd = connection.CreateCommand())
                {
                    createTableCmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Users (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT NOT NULL UNIQUE COLLATE NOCASE,
                    password TEXT NOT NULL
                    );";

                    createTableCmd.ExecuteNonQuery();
                }
            }
        }

        public bool AddUser(string username, string password)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    using (var addUserCmd = connection.CreateCommand())
                    {
                        addUserCmd.CommandText = @"
                    INSERT OR IGNORE INTO Users (username, password)
                    VALUES ($username, $password);";

                        addUserCmd.Parameters.AddWithValue("$username", username);
                        addUserCmd.Parameters.AddWithValue("$password", passwordHash);

                        return addUserCmd.ExecuteNonQuery() > 0;
                    }
                }
        }

        public DatabaseCheckResult CheckIfUserExistsAndGetId(string username)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var checkUserExistsCmd = connection.CreateCommand())
                {
                    checkUserExistsCmd.CommandText = @"
                    SELECT id
                    FROM Users
                    WHERE username = $username;";

                    checkUserExistsCmd.Parameters.AddWithValue("$username", username);

                    object? result = checkUserExistsCmd.ExecuteScalar();

                    if (result == null)
                    {
                        return new DatabaseCheckResult { SearchResult = false }; 
                    }

                    return new DatabaseCheckResult
                    {
                        UserId = Convert.ToInt64(result),
                        SearchResult = true
                    };
                    
                }
            }
        }

        public bool CheckPassword(long userId, string password)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var checkPasswordCmd = connection.CreateCommand())
                {
                    checkPasswordCmd.CommandText = @"
                    SELECT password
                    FROM Users
                    WHERE id = $userid;";

                    checkPasswordCmd.Parameters.AddWithValue("$userid", userId);

                    var result = checkPasswordCmd.ExecuteScalar();

                    if (result == null)
                    {
                        return false;
                    }

                    string passwordHash = Convert.ToString(result) ?? string.Empty;

                    try
                    {
                        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
                    }
                    catch (BCrypt.Net.SaltParseException)
                    {
                        return false;
                    }
                }
            }

        }
    }
}
