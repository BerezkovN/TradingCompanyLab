using BusinessLogic;

namespace UnitTests
{
    public class TradingCompanyTests
    {
        TradingCompany _tradingCompany;
        [SetUp]
        public void Setup()
        {
            _tradingCompany = new TradingCompany("TestServer");   
        }

        [Test]
        public void Login_correctData_test()
        {
            string username = "jane_smith";
            string password = "pass456";

            if (_tradingCompany.LogIn(username, password) == null)
            {
                Assert.Fail();
            }
            else
            {
                Assert.Pass();
            }
            _tradingCompany.LogOut();
        }
        public void Login_wrongData_test()
        {
            string username = "jane_smith";
            string password = "pass4565";

            if (_tradingCompany.LogIn(username, password) == null)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
            _tradingCompany.LogOut();
        }

        [Test]
        public void CheckRecoveryKey_correctData_test()
        {
            
            string username = "jane_smith";
            string password = "pass456";
            string recovery_key = "1111";

            _tradingCompany.LogIn(username, password);

            if (_tradingCompany.CheckRecoveryKey(username, recovery_key))
                Assert.Pass();
            else
                Assert.Fail();

            _tradingCompany.LogOut();

        }
        [Test]
        public void CheckRecoveryKey_wrongData_test()
        {

            string username = "jane_smith";
            string password = "pass456";
            string recovery_key = "2211";

            _tradingCompany.LogIn(username, password);

            if (_tradingCompany.CheckRecoveryKey(username, recovery_key))
                Assert.Fail();
            else
                Assert.Pass();


            _tradingCompany.LogOut();

        }
        [Test]
        public void CheckUpdatePassword_test()
        {

            string username = "grace_field";
            string password = "graceful123";
            string recoveryKey = "1111";
            
            _tradingCompany.LogIn(username, password);

            string new_password = "graceful1234";

            _tradingCompany.UpdatePassword(username, recoveryKey, new_password);

            _tradingCompany.LogOut();
            _tradingCompany.LogIn(username, new_password);

            if (_tradingCompany.LoggedInUser == null )
                Assert.Fail();
            else
                Assert.Pass();

            _tradingCompany.UpdatePassword(username, recoveryKey, password);
            _tradingCompany.LogOut();

        }
        [Test]
        public void GetAllUser_test()
        {

            if (_tradingCompany.GetAllUsers() == null)
                Assert.Fail();
            else
                Assert.Pass();
        }
        [Test]
        public void GetAllUserSessions_test()
        {

            if (_tradingCompany.GetAllUserSessions() == null)
                Assert.Fail();
            else
                Assert.Pass();
        }

    }
}