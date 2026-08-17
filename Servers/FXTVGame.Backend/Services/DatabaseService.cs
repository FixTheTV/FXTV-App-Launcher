using FXTVGame.Backend.Models;
using MySqlConnector;


namespace FXTVGame.Backend.Services
{
    public class DatabaseService
    {
        private readonly string connectionString = "Server=localhost;Port=3306;Database=fxtv_launcher;User=root;Password=;Pooling=true;Min Pool Size=5;Max Pool Size=100;";

        public DatabaseService()
        {
            Initialize();
        }

        public void Initialize()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                    connection.Open();

                    using (var createTableCmd = connection.CreateCommand())
                    {
                        createTableCmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS users (
                        id BIGINT AUTO_INCREMENT PRIMARY KEY,
                        username VARCHAR(50) NOT NULL UNIQUE,
                        password VARCHAR(255) NOT NULL,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;";

                        createTableCmd.ExecuteNonQuery();
                    }
              


            }
        }

        public async Task<bool> AddUserAsync(string username, string password)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var addUserCmd = connection.CreateCommand())
                {
                    addUserCmd.CommandText = @"
                    INSERT IGNORE INTO users (username, password)
                    VALUES (@username, @password);";

                    addUserCmd.Parameters.AddWithValue("@username", username);
                    addUserCmd.Parameters.AddWithValue("@password", passwordHash);

                    int rowsAffected = await addUserCmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<DatabaseCheckResult> CheckIfUserExistsAndGetIdAsync(string username)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var checkUserExistsCmd = connection.CreateCommand())
                {
                    checkUserExistsCmd.CommandText = @"
                    SELECT id
                    FROM users
                    WHERE username = @username;";

                    checkUserExistsCmd.Parameters.AddWithValue("@username", username);

                    object? result = await checkUserExistsCmd.ExecuteScalarAsync();

                    if (result == null || result == DBNull.Value)
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

        public async Task<bool> CheckPasswordAsync(long userId, string password)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var checkPasswordCmd = connection.CreateCommand())
                {
                    checkPasswordCmd.CommandText = @"
                    SELECT password
                    FROM users
                    WHERE id = @userid;";

                    checkPasswordCmd.Parameters.AddWithValue("@userid", userId);

                    var result = await checkPasswordCmd.ExecuteScalarAsync();

                    if (result == null || result == DBNull.Value)
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