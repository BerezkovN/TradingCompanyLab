using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using DAL.AdoNet;
using Microsoft.Extensions.Configuration;
using DTO;
namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        
        [TestMethod]
        public void Add_ShouldReturnCorrectSum()
        {
            
            var result = 5 + 3;

            
            Assert.AreEqual(8, result);
        }
        [TestMethod]
        public void Add_ShouldReturnCorrectSum2()
        {

            var result = 5 + 3;


            Assert.AreEqual(7, result);
        }
    }
}




/*private SqlConnection _connection;
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
            //Assert.AreEqual(userUsername, user.Username);
        }*/