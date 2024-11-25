using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using DAL.Interface;
using DTO;

namespace DAL.AdoNet
{
    public class UserDal : IUserDal
    {
        private readonly string _connectionString;

        public UserDal(string connection)
        {
            _connectionString = connection;
        }

        public UserData? Login(string username, string password)
        {
            UserData? user = null;
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = connection.CreateCommand();
                
                command.CommandText =
                    @"SELECT u.Id, u.Username, u.Email, u.Password, u.FirstName, u.LastName, u.Gender, 
                                u.PhoneNumber, u.Address, u.Role, u.RecoveryKey, 
                                u.ProfilePicture, u.CreatedAt, u.UpdatedAt
                        FROM UsersTBL u
                        WHERE u.Username = @Username AND u.Password = @Password";

                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
            
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                
                if (reader.Read())
                {
                    user = new UserData
                    {
                        UserId = Convert.ToInt32(reader["Id"]),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        Email = reader["Email"].ToString(),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        Role = reader["Role"].ToString(),
                        RecoveryKey = reader["RecoveryKey"].ToString(),
                        ProfilePicture = reader["ProfilePicture"] as byte[],
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"])
                    };
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error when logging in: {ex.Message}");
            }

            return user;
        }


        public void UpdateUser(string columnName, object newValue, int userId)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = connection.CreateCommand();

                command.CommandText = $"UPDATE UsersTBL SET {columnName} = @newValue WHERE Id = @UserId";
                command.Parameters.AddWithValue("@newValue", newValue);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error when updating a user: {ex.Message}");
            }
        }

        public void DeleteUser(int userId)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = connection.CreateCommand();
                
                connection.Open();

                // Видалення записів із залежних таблиць
                using (SqlCommand deleteSessionsCommand = connection.CreateCommand())
                {
                    deleteSessionsCommand.CommandText = "DELETE FROM SessionsTBL WHERE UserId = @UserId";
                    deleteSessionsCommand.Parameters.AddWithValue("@UserId", userId);
                    deleteSessionsCommand.ExecuteNonQuery();
                }

                // Видалення самого користувача
                command.CommandText = "DELETE FROM UsersTBL WHERE Id = @UserId";
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error when deleting a user: {ex.Message}");
            }
        }

        public List<UserData> GetAllUsers()
        {
            List<UserData> users = new List<UserData>();

            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM UsersTBL";

                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new UserData
                    {
                        UserId = Convert.ToInt32(reader["Id"]),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        Email = reader["Email"].ToString(),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        Role = reader["Role"].ToString(),
                        RecoveryKey = reader["RecoveryKey"].ToString(),
                        ProfilePicture = reader["ProfilePicture"] as byte[],
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"])
                    });
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error when getting all users: {ex.Message}");
            }

            return users;
        }

        public UserData? GetUser(int userId)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = connection.CreateCommand();

                command.CommandText = "SELECT Id, Username, Email, Role, FirstName, LastName, Gender, PhoneNumber, Address, ProfilePicture FROM UsersTBL WHERE Id = @UserId";
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new UserData
                    {
                        UserId = reader.GetInt32(0),
                        Username = !reader.IsDBNull(1) ? reader.GetString(1) : null,
                        Email = !reader.IsDBNull(2) ? reader.GetString(2) : null,
                        Role = !reader.IsDBNull(3) ? reader.GetString(3) : null,
                        FirstName = !reader.IsDBNull(4) ? reader.GetString(4) : null,
                        LastName = !reader.IsDBNull(5) ? reader.GetString(5) : null,
                        Gender = !reader.IsDBNull(6) ? reader.GetString(6) : null,
                        PhoneNumber = !reader.IsDBNull(7) ? reader.GetString(7) : null,
                        Address = !reader.IsDBNull(8) ? reader.GetString(8) : null,
                        ProfilePicture = !reader.IsDBNull(9) ? (byte[])reader[9] : null
                    };
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error when getting a user: {ex.Message}");
            }

            // Повертаємо null, якщо користувача не знайдено
            return null; 
        }

       
        public void CreateUser(string username, string email, string password, string recoveryKey)
        { 
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = connection.CreateCommand();

                command.CommandText = @"INSERT INTO UsersTBL (Username, Email, Password, RecoveryKey, Role, CreatedAt)
                                              VALUES (@Username, @Email, @Password, @RecoveryKey, @Role, @CreatedAt)";

                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@RecoveryKey", recoveryKey);
                        command.Parameters.AddWithValue("@Role", "User");
                        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when create user: {ex.Message}");
                throw;
            }
            
        }

        public UserData? GetUserByUsername(string username)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Username, Email, Role, FirstName, LastName, Gender, PhoneNumber, Address, ProfilePicture, RecoveryKey FROM UsersTBL WHERE Username = @Username";
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new UserData
                    {
                        UserId = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Email = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Role = reader.GetString(3),
                        FirstName = reader.IsDBNull(4) ? null : reader.GetString(4),
                        LastName = reader.IsDBNull(5) ? null : reader.GetString(5),
                        Gender = reader.IsDBNull(6) ? null : reader.GetString(6),
                        PhoneNumber = reader.IsDBNull(7) ? null : reader.GetString(7),
                        Address = reader.IsDBNull(8) ? null : reader.GetString(8),
                        ProfilePicture = reader.IsDBNull(9) ? null : (byte[])reader[9],
                        RecoveryKey = reader.IsDBNull(10) ? null : reader.GetString(10)
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка зчитування користувача по імені: {ex.Message}");
                throw;
            }

            return null; // Повертаємо null, якщо користувача не знайдено
        }

    }
}
