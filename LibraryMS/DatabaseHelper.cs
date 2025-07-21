using System;
using System.Data.SQLite;
using System.IO;

namespace LibraryMS
{
    public static class DatabaseHelper
    {
        private static string dbFile = "library.db";
        private static string connectionString = $"Data Source={dbFile};Version=3;";

        public static void InitializeDatabase()
        {
            if (!File.Exists(dbFile))
            {
                SQLiteConnection.CreateFile(dbFile);
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string sql = @"
                    CREATE TABLE Roles (
                        role_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        role_name TEXT NOT NULL
                    );

                    CREATE TABLE Users (
                        user_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        email TEXT NOT NULL UNIQUE,
                        password_hash TEXT NOT NULL,
                        role_id INTEGER NOT NULL,
                        FOREIGN KEY (role_id) REFERENCES Roles (role_id)
                    );

                    CREATE TABLE Authors (
                        author_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL
                    );

                    CREATE TABLE Genres (
                        genre_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL
                    );

                    CREATE TABLE Books (
                        book_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        title TEXT NOT NULL,
                        author_id INTEGER,
                        genre_id INTEGER,
                        year_published INTEGER,
                        copies_total INTEGER NOT NULL,
                        copies_available INTEGER NOT NULL,
                        FOREIGN KEY (author_id) REFERENCES Authors (author_id),
                        FOREIGN KEY (genre_id) REFERENCES Genres (genre_id)
                    );

                    CREATE TABLE Borrowings (
                        borrowing_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        user_id INTEGER NOT NULL,
                        book_id INTEGER NOT NULL,
                        borrow_date DATE NOT NULL,
                        return_date DATE,
                        returned INTEGER NOT NULL DEFAULT 0,
                        FOREIGN KEY (user_id) REFERENCES Users (user_id),
                        FOREIGN KEY (book_id) REFERENCES Books (book_id)
                    );
                    ";

                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    string insertRoles = @"
                        INSERT INTO Roles (role_name) VALUES ('Admin');
                        INSERT INTO Roles (role_name) VALUES ('User');
                    ";

                    using (var command = new SQLiteCommand(insertRoles, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                Console.WriteLine("Database created and tables initialized.");
            }
            else
            {
                Console.WriteLine("Database already exists.");
            }
        }

        public static bool UserExists(string email)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE email = @Email";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    long count = (long)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public static bool RegisterUser(string fullName, string email, string passwordHash)
        {
            if (UserExists(email))
                return false;

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string insertQuery = @"
                    INSERT INTO Users (name, email, password_hash, role_id)
                    VALUES (@Name, @Email, @PasswordHash, 2);
                ";

                using (var cmd = new SQLiteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", fullName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public static bool VerifyLogin(string email, string passwordHash)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM Users WHERE email = @Email AND password_hash = @PasswordHash";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    long count = (long)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public static User GetUserByEmail(string email)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT user_id, name, email, password_hash, role_id FROM Users WHERE email = @Email";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = Convert.ToInt32(reader["user_id"]),
                                Name = reader["name"].ToString(),
                                Email = reader["email"].ToString(),
                                Password = reader["password_hash"].ToString(),
                                RoleId = Convert.ToInt32(reader["role_id"])
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
