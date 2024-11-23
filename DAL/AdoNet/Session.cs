using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Interface;
using DTO;

namespace DAL.AdoNet
{
    public class Session : ISessionDal
    {
        private readonly SqlConnection _connection;

        public Session(SqlConnection connection)
        {
            _connection = connection;
        }
        public void EndSession(int userId)
        {
            try
            {
                using (SqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = "UPDATE SessionsTBL SET Status = @Status, LogoutTime = @LogoutTime WHERE UserId = @UserId AND Status = 'Logged in'";
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Status", "Logged out");
                    command.Parameters.AddWithValue("@LogoutTime", DateTime.Now);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ending session: {ex.Message}");
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public void StartSession(int userId)
        {
            try
            {
                using (SqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO SessionsTBL (UserId, Status, LoginTime, LogoutTime) VALUES (@UserId, @Status, @LoginTime, NULL)";
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Status", "Logged in");
                    command.Parameters.AddWithValue("@LoginTime", DateTime.Now);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting session: {ex.Message}");
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public List<(int UserId, string Username, string Status, DateTime? LoginTime, DateTime? LogoutTime)> GetUserSessions()
        {
            List<(int UserId, string Username, string Status, DateTime? LoginTime, DateTime? LogoutTime)> sessions = new List<(int, string, string, DateTime?, DateTime?)>();

            try
            {
                using (SqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = @"
                SELECT 
                    UsersTBL.Id, 
                    UsersTBL.Username, 
                    SessionsTBL.Status, 
                    SessionsTBL.LoginTime, 
                    SessionsTBL.LogoutTime
                FROM 
                    UsersTBL
                LEFT JOIN 
                    SessionsTBL 
                ON 
                    UsersTBL.Id = SessionsTBL.UserId";

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sessions.Add((
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                                reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving sessions: {ex.Message}");
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return sessions;
        }



        public List<SessionData> GetUserSessions(int userId)
        {
            List<SessionData> sessions = new List<SessionData>();

            try
            {
                using (SqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM SessionsTBL WHERE UserId = @UserId";
                    command.Parameters.AddWithValue("@UserId", userId);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sessions.Add(new SessionData
                            {
                                UserId = Convert.ToInt32(reader["UserId"]),
                                Status = reader["Status"].ToString(),
                                LoginTime = Convert.ToDateTime(reader["LoginTime"]),
                                LogoutTime = reader.IsDBNull(reader.GetOrdinal("LogoutTime")) ? (DateTime?)null : Convert.ToDateTime(reader["LogoutTime"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка отримання сесій: {ex.Message}");
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return sessions;
        }

    }
}
