/*using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using DAL.AdoNet;
using Microsoft.Extensions.Configuration;

[TestClass]
public class SessionTests
{
    private SqlConnection _connection;
    private Session _session;

    [TestInitialize]
    public void Setup()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("conFile.json")
            .Build();

        string connectionString = configuration.GetConnectionString("Tests");
        _connection = new SqlConnection(connectionString);
        _session = new Session(_connection);
    }

    [TestMethod]
    public void StartSession_ValidUserId_InsertsSession()
    {
        // Arrange
        int testUserId = 1;

        // Act
        _session.StartSession(testUserId);

        // Assert
        using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM SessionsTBL WHERE UserId = @UserId AND Status = 'Logged in'", _connection))
        {
            command.Parameters.AddWithValue("@UserId", testUserId);
            _connection.Open();
            int sessionCount = (int)command.ExecuteScalar();
            _connection.Close();

            Assert.AreEqual(1, sessionCount, "Session was not inserted correctly.");
        }
    }

    [TestMethod]
    public void EndSession_ValidUserId_UpdatesSession()
    {
        // Arrange
        int testUserId = 1;
        _session.StartSession(testUserId);

        // Act
        _session.EndSession(testUserId);

        // Assert
        using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM SessionsTBL WHERE UserId = @UserId AND Status = 'Logged out'", _connection))
        {
            command.Parameters.AddWithValue("@UserId", testUserId);
            _connection.Open();
            int sessionCount = (int)command.ExecuteScalar();
            _connection.Close();

            Assert.AreEqual(1, sessionCount, "Session was not updated correctly.");
        }
    }
}
*/