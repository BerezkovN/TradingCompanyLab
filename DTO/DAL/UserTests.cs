/*using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using DAL.AdoNet;
using Microsoft.Extensions.Configuration;
using DTO;

[TestClass]
public class UserTests
{
    private SqlConnection _connection;
    private User _user;

    [TestInitialize]
    public void Setup()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("conFile.json")
            .Build();

        string connectionString = configuration.GetConnectionString("Tests");
        _connection = new SqlConnection(connectionString);
        _user = new User(_connection);
    }
    [TestMethod]
    public void GetUserByUsername_ShouldReturnUser_WhenUserExists()
    {

        var userUsername = "john_doe";

        var user = _user.GetUserByUsername(userUsername);

        Assert.IsNotNull(user);
        Assert.AreEqual(userUsername, user.Username);
    }
    
    [TestMethod]
    public void Login_ValidCredentials_ReturnsUser()
    {
        // Arrange
        string testUsername = "testUser";
        string testPassword = "testPassword";

        // Act
        var result = _user.Login(testUsername, testPassword);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(testUsername, result.Username, "Username does not match.");
    }

    [TestMethod]
    public void GetAllUsers_ReturnsNonEmptyList()
    {
        // Act
        var users = _user.GetAllUsers();

        // Assert
        Assert.IsTrue(users.Count > 0, "Users list should not be empty.");
    }*/





    //public UserData GetUserByUsername(string username);