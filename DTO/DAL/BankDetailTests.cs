/*using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using DAL.AdoNet;
using Microsoft.Extensions.Configuration;

[TestClass]
public class BankDetailTests
{
    private SqlConnection _connection;
    private BankDetail _bankDetail;

    [TestInitialize]
    public void Setup()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("conFile.json")
            .Build();

        string connectionString = configuration.GetConnectionString("Tests");
        _connection = new SqlConnection(connectionString);
        _bankDetail = new BankDetail(_connection);
    }

    [TestMethod]
    public void GetBankDetailData_ValidUserId_ReturnsBankDetails()
    {
        // Arrange
        int testUserId = 1;

        // Act
        var result = _bankDetail.GetBankDetailData(testUserId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(testUserId, result.UserId, "UserId does not match.");
    }

    [TestMethod]
    public void IsValidCardNumber_ValidCardNumber_ReturnsTrue()
    {
        // Arrange
        string validCardNumber = "4111111111111111";

        // Act
        bool isValid = BankDetail.IsValidCardNumber(validCardNumber);

        // Assert
        Assert.IsTrue(isValid, "Card number validation failed.");
    }
}

using DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

[TestClass]
public class UserDalTests
{
    private readonly string connStr;

    public UserDalTests()
    {
        connStr = ConfigurationManager.ConnectionStrings["TradingCompanyVicky_Test"].ConnectionString;
    }

    [TestMethod]
    public void GetAll_ShouldReturnUsers()
    {
        var userDal = new UserDal(connStr);

        var users = userDal.GetAll();

        Assert.IsNotNull(users);
        Assert.IsTrue(users.Count > 0);
    }
    [TestMethod]
    public void GetById_ShouldReturnUser_WhenUserExists()
    {

        var userDal = new UserDal(connStr);
        var userId = 2;

        var user = userDal.GetById(userId);

        Assert.IsNotNull(user);
        Assert.AreEqual(userId, user.UserId);
    }
    [TestMethod]
    public void Insert_ShouldAddUser()
    {
        var userDal = new UserDal(connStr);
        var user = new UserDto { UserName = "Te212stU12ser1", Email = "te1s12t@te1st.c123om", Password = "p13" };

        userDal.Insert(user);
        var users = userDal.GetAll();

        Assert.IsTrue(users.Exists(u => u.UserName == user.UserName && u.Email == user.Email));
    }

}*/