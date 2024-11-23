using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Interface;
using DTO;

namespace DAL.AdoNet
{
    public class User : IUserDal
    {
        private readonly SqlConnection _connection;

        public User(SqlConnection connection)
        {
            _connection = connection;
        }

        public UserData Login(string username, string password)
        {
            UserData user = null;
            try
            {
                using (SqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = @"SELECT u.Id, u.Username, u.Email, u.FirstName, u.LastName, u.Gender, 
                                              u.PhoneNumber, u.Address, u.Role, u.RecoveryKey, 
                                              u.ProfilePicture, u.CreatedAt, u.UpdatedAt, 
                                              b.CardNumber, b.ExpirationDate, b.CardCVV, b.CardHolderName
                                        FROM UsersTBL u
                                        LEFT JOIN BankDetailsTBL b ON u.Id = b.UserId
                                        WHERE u.Username = @Username AND u.Password = @Password";

                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new UserData
                            {
                                UserId = Convert.ToInt32(reader["Id"]),
                                Username = reader["Username"].ToString(),
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
                                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"]),
                                BankCardDetails = new BankDetailData
                                {
                                    CardNumber = reader["CardNumber"].ToString(),
                                    ExpirationDate = reader["ExpirationDate"].ToString(),
                                    CardCVV = reader["CardCVV"].ToString(),
                                    CardHolderName = reader["CardHolderName"].ToString()
                                }
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка авторизації користувача: {ex.Message}");
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return user;
        }


        public void UpdateUser(string columnName, object newValue, int userId)
        {
            try
            {
                using (SqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE UsersTBL SET {columnName} = @newValue WHERE Id = @UserId";
                    command.Parameters.AddWithValue("@newValue", newValue);
                    command.Parameters.AddWithValue("@UserId", userId);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка оновлення користувача: {ex.Message}");
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public void DeleteUser(int userId)
        {
            try
            {
                using (SqlCommand command = _connection.CreateCommand())
                {
                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    // Видалення записів із залежних таблиць
                    using (SqlCommand deleteSessionsCommand = _connection.CreateCommand())
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка видалення користувача: {ex.Message}");
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }


        public List<UserData> GetAllUsers()
        {
            List<UserData> users = new List<UserData>();

            try
            {
                using (SqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM UsersTBL";

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new UserData
                            {
                                UserId = Convert.ToInt32(reader["Id"]),
                                Username = reader["Username"].ToString(),
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка отримання списку користувачів: {ex.Message}");
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return users;
        }
        public UserData GetUser(int userId)
        {
            try
            {
                using (SqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Username, Email, Role, FirstName, LastName, Gender, PhoneNumber, Address, ProfilePicture FROM UsersTBL WHERE Id = @UserId";
                    command.Parameters.AddWithValue("@UserId", userId);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user: {ex.Message}");
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return null; // Повертаємо null, якщо користувача не знайдено
        }
        public UserData GetUserByUsername(string username)
        {
            try
            {
                using (SqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Username, Email, Role, FirstName, LastName, Gender, PhoneNumber, Address, ProfilePicture, RecoveryKey FROM UsersTBL WHERE Username = @Username";
                    command.Parameters.AddWithValue("@Username", username);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user by username: {ex.Message}");
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return null; // Повертаємо null, якщо користувача не знайдено
        }



    }
}
