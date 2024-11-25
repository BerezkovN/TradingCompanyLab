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
    internal class SessionTests
    {
        ISessionDal _sessionDal;
        TradingCompany _tradingCompany;

        [SetUp]
        public void Setup()
        {
            _tradingCompany = new TradingCompany("TestServer");
            _sessionDal = _tradingCompany.Database.SessionDal;
        }
        
        [Test]
        public void EndSession_test()
        {
            string username = "james_smith";
            string email = "james@gmail.com";
            string password = "password456";
            string recoveryKey = "1111";


            _tradingCompany.Database.UserDal.CreateUser(username, email, password, recoveryKey);

            DTO.UserData user = _tradingCompany.Database.UserDal.Login(username, password);

             _tradingCompany.LogIn(username, password);

            _sessionDal.EndSession(user.UserId);

            List<SessionData> userSessions = _sessionDal.GetUserSessions(user.UserId);
            _tradingCompany.Database.UserDal.DeleteUser(user.UserId);
            if (userSessions[userSessions.Count() - 1].Status == "Logged out")
                Assert.Pass();
            else
                Assert.Fail();

        }

        [Test]
        public void StartSession_test()
        {
            string username = "james_smith";
            string email = "james@gmail.com";
            string password = "password456";
            string recoveryKey = "1111";


            _tradingCompany.Database.UserDal.CreateUser(username, email, password, recoveryKey);

            DTO.UserData user = _tradingCompany.Database.UserDal.Login(username, password);

            _tradingCompany.LogIn(username, password);
 

            _sessionDal.EndSession(user.UserId);
            _sessionDal.StartSession(user.UserId);


            List<SessionData> userSessions = _sessionDal.GetUserSessions(user.UserId);

            _tradingCompany.Database.UserDal.DeleteUser(user.UserId);
            if (userSessions[userSessions.Count() -1].Status == "Logged in")
                Assert.Pass();
            else
                Assert.Fail();
        }

        

        
        
        [Test]
        public void GetUserSession_test()
        {
            string username = "james_smith";
            string email = "james@gmail.com";
            string password = "password456";
            string recoveryKey = "1111";


            _tradingCompany.Database.UserDal.CreateUser(username, email, password, recoveryKey);

            DTO.UserData user = _tradingCompany.Database.UserDal.Login(username, password);

            List<SessionData> userSessions = _sessionDal.GetUserSessions(user.UserId);

            if (userSessions == null)
            {
                _tradingCompany.Database.UserDal.DeleteUser(user.UserId);
                Assert.Fail();
            }

            foreach(SessionData session in userSessions)
                {
                if (session.UserId != user.UserId)
                    Assert.Fail();
                }
            _tradingCompany.Database.UserDal.DeleteUser(user.UserId);
            Assert.Pass();
        }
        [Test]
        public void GetAllSession_test()
        {

            if (_sessionDal.GetUserSessions() == null)
                Assert.Fail();
            else
                Assert.Pass();
        }
    }
}
