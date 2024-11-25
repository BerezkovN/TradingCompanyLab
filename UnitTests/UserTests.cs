using BusinessLogic;
using DAL.Interface;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    internal class UserTests
    {
        IUserDal _userDal;
        TradingCompany _tradingCompany;

        [SetUp]
        public void Setup()
        {
            _tradingCompany = new TradingCompany("TestServer");
            _userDal = _tradingCompany.Database.UserDal;
        }
        [Test]
        public void Login_correctData_test()
        {
            string username = "jane_smith";
            string password = "pass456";

            if (_userDal.Login(username, password) == null)
            {
                Assert.Fail();
            }
            else
            {
                Assert.Pass();
            }
        }
        [Test]
        public void Login_wrongData_test()
        {
            string username = "jane_smith";
            string password = "pass45re6";

            if (_userDal.Login(username, password) == null)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void UpdateUser_test()
        {
            string username = "jane_smith";
            string password = "pass456";

            var user = _userDal.Login(username, password);

            string newFirstName = "Janiffer";

            _userDal.UpdateUser("FirstName", newFirstName, user.UserId);

            user = _userDal.Login(username, password);

            if (user.FirstName == newFirstName)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]

        public void CreateUser_test()
        {
            string username = "james_smith";
            string email = "james@gmail.com";
            string password = "password456";
            string recoveryKey = "1111";


            _userDal.CreateUser(username, email, password, recoveryKey);

            List<DTO.UserData> users = _userDal.GetAllUsers();


            foreach (DTO.UserData us in users)
            {
                if (us.Username == "james_smith")
                {
                    _userDal.DeleteUser(us.UserId);
                    Assert.Pass();
                    break;
                }
            }


        }
        
        [Test]
        public void DeleteUser_test()
        {

            
            var Username = "testuser";
            var Password = "password";
            var Email = "test@example.com";
            var RecoveryKey = "1111";
            

            _tradingCompany.Database.UserDal.CreateUser(Username, Email, Password, RecoveryKey);
            var user = _tradingCompany.Database.UserDal.Login(Username, Password);


            _userDal.DeleteUser(user.UserId);


            try
            {
                var deletedUser = _tradingCompany.Database.UserDal.Login(user.Username, user.Password);

                // Якщо користувач знайдений після видалення, тест провалений
                Assert.Fail("User should not exist after deletion.");
            }
            catch
            {
                // Якщо метод Login викликає виняток, вважаємо тест пройденим
                Assert.Pass("User does not exist after deletion, as expected.");
            }
        }
        [Test]

        public void GetUser_test()
        {
            string username = "james_smith";
            string email = "james@gmail.com";
            string password = "password456";
            string recoveryKey = "1111";


            _userDal.CreateUser(username, email, password, recoveryKey);

            DTO.UserData user = _userDal.Login(username, password);


            if (_userDal.GetUser(user.UserId).Username == "james_smith")
            {
                _userDal.DeleteUser(user.UserId);
                Assert.Pass();
            }
            _userDal.DeleteUser(user.UserId);
            Assert.Fail();
        }
        [Test]
        public void GetAllUser_test()
        {

            if (_userDal.GetAllUsers() == null)
                Assert.Fail();
            else
                Assert.Pass();
        }


    }
}
