﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Interface;
using DTO;

namespace DAL.AdoNet
{
    public class SessionDal : ISessionDal
    {
        private readonly string _connectionString;

        public SessionDal(string connection)
        {
            _connectionString = connection;
        }

        public void EndSession(int userId)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE SessionsTBL SET Status = @Status, LogoutTime = @LogoutTime WHERE UserId = @UserId AND Status = 'Logged in'";
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Status", "Logged out");
                command.Parameters.AddWithValue("@LogoutTime", DateTime.Now);

                connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ending session: {ex.Message}");
                throw;
            }
        }

        public void StartSession(int userId)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = connection.CreateCommand();
                
                command.CommandText = "INSERT INTO SessionsTBL (UserId, Status, LoginTime, LogoutTime) VALUES (@UserId, @Status, @LoginTime, NULL)";
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Status", "Logged in");
                command.Parameters.AddWithValue("@LoginTime", DateTime.Now);
                
                connection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Помилка при початку сесії: {ex.Message}");
            }
        }

        public List<SessionData> GetUserSessions()
        {
            List<SessionData> sessions = new List<SessionData>();

            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                
                using SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                SELECT 
                    UsersTBL.Id, 
                    SessionsTBL.Status, 
                    SessionsTBL.LoginTime, 
                    SessionsTBL.LogoutTime
                FROM 
                    UsersTBL
                LEFT JOIN 
                    SessionsTBL 
                ON 
                    UsersTBL.Id = SessionsTBL.UserId";

                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    sessions.Add(new SessionData {
                        UserId = reader.GetInt32(0),
                        Status = reader.GetString(1),
                        LoginTime = reader.GetDateTime(2),
                        LogoutTime = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Помилка зчитування сесії: {ex.Message}");
            }

            return sessions;
        }


        public List<SessionData> GetUserSessions(int userId)
        {
            List<SessionData> sessions = new List<SessionData>();

            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = connection.CreateCommand();
                
                command.CommandText = "SELECT * FROM SessionsTBL WHERE UserId = @UserId";
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
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
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Помилка отримання сесій: {ex.Message}");
            }

            return sessions;
        }

    }
}