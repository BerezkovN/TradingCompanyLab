using BusinessLogic;
using DAL.AdoNet;
using DAL.Interface;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class BankDetailTests
    {

        TradingCompany _tradingCompany;

        [SetUp]
        public void Setup()
        {
            _tradingCompany = new TradingCompany("TestServer");
        }

        [Test]
        public void UpdateBankDetail_test()
        {
            string username = "jane_smith";
            string password = "pass456";

            var user = _tradingCompany.Database.UserDal.Login(username, password);

            var newBankDetails = new DTO.BankDetailData
            {
                UserId = user.UserId,
                CardNumber = "4444444444444444",
                ExpirationDate = "2025-11-30",
                CardCVV = "455",
                CardHolderName = "Janiffer Smith",
                BillingAddress = "456 Oak St, City B, Country Y"
            };

            _tradingCompany.Database.BankDetailDal.UpdateBankDetail(newBankDetails);

            var updatedUser = _tradingCompany.Database.UserDal.Login(username, password);

            var retrievedDetails = _tradingCompany.GetBankDetail(new User(user));

            Assert.NotNull(retrievedDetails, "Bank details should not be null.");
            Assert.AreEqual(_tradingCompany.Database.BankDetailDal.GetBankDetailData(updatedUser.UserId).CardNumber, retrievedDetails.CardNumber, "Card numbers should match.");
        }

        
        [Test]
        public void GetBankDetail_test()
        {
            // Arrange
            var _user = new UserData
            {
                Username = "testuser",
                Password = "password",
                Email = "test@example.com",
                Role = "User"
            };

            

            _tradingCompany.Database.UserDal.CreateUser(_user.Username, _user.Email, _user.Password, "1111");
            var user = _tradingCompany.Database.UserDal.Login(_user.Username, _user.Password);

            var bankDetails = new BankDetailData
            {
                UserId = user.UserId,
                CardNumber = "1234567890123456",
                ExpirationDate = "2026-12-25",
                CardCVV = "123",
                CardHolderName = "Test User",
                BillingAddress = "123 Test St"
            };
 /*           _tradingCompany.Database.BankDetailDal.UpdateBankDetail(bankDetails);

            var updatedUser = _tradingCompany.Database.UserDal.Login(username, password);

            var retrievedDetails = _tradingCompany.GetBankDetail(new User(user));

            Assert.NotNull(retrievedDetails, "Bank details should not be null.");
            Assert.AreEqual(_tradingCompany.Database.BankDetailDal.GetBankDetailData(updatedUser.UserId).CardNumber, retrievedDetails.CardNumber, "Card numbers should match.");
 */

            _tradingCompany.Database.BankDetailDal.UpdateBankDetail(bankDetails);

           
             var retrievedDetails = _tradingCompany.Database.BankDetailDal.GetBankDetailData(user.UserId);
            _tradingCompany.Database.UserDal.DeleteUser(user.UserId);
  
            Assert.NotNull(retrievedDetails, "Bank details should not be null.");
            Assert.AreEqual(bankDetails.CardNumber, retrievedDetails.CardNumber, "Card numbers should match.");
        }
    }
}
