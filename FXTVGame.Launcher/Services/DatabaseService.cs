using Microsoft.Data.Sqlite;

namespace FXTVGame.Launcher.Services.Database
{
    public class DatabaseService
    {
        private readonly string connectionString = "Data Source=MyDatabase.db;";

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

        public void AddUser(string username, string password)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var insertCmd = connection.CreateCommand())
                {
                    insertCmd.CommandText = @"
                    INSERT OR IGNORE INTO Users (username, password)
                    VALUES ($username, $password);";

                    insertCmd.Parameters.AddWithValue("$username", username);
                    insertCmd.Parameters.AddWithValue("$password", password);

                    insertCmd.ExecuteNonQuery();
                }
            }
        }

        public bool UserExists(string username, string password)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var selectCmd = connection.CreateCommand())
                {
                    selectCmd.CommandText = @"
                    SELECT COUNT(*)
                    FROM Users
                    WHERE username = $username AND password = $password;";

                    selectCmd.Parameters.AddWithValue("$username", username);
                    selectCmd.Parameters.AddWithValue("$password", password);

                    object? result = selectCmd.ExecuteScalar();
                    long count = Convert.ToInt64(result);

                    return count > 0;
                }
            }
        }
    }
}