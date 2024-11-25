using DAL.AdoNet;
using DAL.Interface;
using DTO;
using Microsoft.Extensions.Configuration;

namespace BusinessLogic
{
    public class TradingCompany
    {

        private readonly IDatabase _database;
        private User? _loggedInUser;

        public IDatabase Database => _database;
        public User? LoggedInUser => _loggedInUser;

        public TradingCompany() 
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();

            string? connectionString = configuration.GetConnectionString("SqlServer");
            if (connectionString == null)
            {
                throw new InvalidOperationException("Could not find config.json");
            }

            _database = new DAL.AdoNet.Database(connectionString);
        }

        public User? LogIn(string username, string password)
        {
            var userData = _database.UserDal.Login(username, password);

            if (userData == null)
            {
                return null;
            }

            // Створення сесії після успішного входу
            _database.SessionDal.StartSession(userData.UserId);

            _loggedInUser = new User(userData);
            _loggedInUser.BankDetailData = _database.BankDetailDal.GetBankDetailData(userData.UserId);

            return _loggedInUser;
        }

        public bool CheckRecoveryKey(string recoveryKey)
        {
            if (_loggedInUser == null)
                return false;

            return _loggedInUser.Data.RecoveryKey == recoveryKey;
        }

        public void UpdatePassword(string recoveryKey, string newPassword)
        {
            if (_loggedInUser == null)
                return;

            if (!CheckRecoveryKey(recoveryKey))
                return;

            _database.UserDal.UpdateUser("Password", newPassword, _loggedInUser.Data.UserId);

        }

        public List<User> GetAllUsers()
        {

            var result = new List<User>();
            List<UserData> users = _database.UserDal.GetAllUsers();

            foreach (var userData in users)
            {
                result.Add(new User(userData));
            }

            return result;
        }

        public User GetUser(int userId)
        {
            return new User(_database.UserDal.GetUser(userId));
        }

        public void UpdateUser(User user)
        {
            this.UpdateUser(user, nameof(user.Data.Username), user.Data.Username);
            this.UpdateUser(user, nameof(user.Data.Email), user.Data.Email);
            this.UpdateUser(user, nameof(user.Data.FirstName), user.Data.FirstName);
            this.UpdateUser(user, nameof(user.Data.LastName), user.Data.LastName);
            this.UpdateUser(user, nameof(user.Data.Gender), user.Data.Gender);
            this.UpdateUser(user, nameof(user.Data.PhoneNumber), user.Data.PhoneNumber);
            this.UpdateUser(user, nameof(user.Data.Address), user.Data.Address);

            this.UpdateBankDetail(user.BankDetailData);
        }

        public void UpdateUser(User user, string columnName, object? value)
        {
            if (value == null)
                return;

            _database.UserDal.UpdateUser(columnName, value, user.Data.UserId);
            _database.UserDal.UpdateUser("UpdatedAt", DateTime.Now, user.Data.UserId);
        }

        public void DeleteUser(User user)
        {
            _database.UserDal.DeleteUser(user.Data.UserId);
        }

        public BankDetailData? GetBankDetail(User user)
        {
            return _database.BankDetailDal.GetBankDetailData(user.Data.UserId);
        }

        public void UpdateBankDetail(BankDetailData data)
        {
            _database.BankDetailDal.UpdateBankDetail(data);
        }

        public List<SessionData> GetAllUserSessions()
        {
            return _database.SessionDal.GetUserSessions();
        }

        public void EndUserSession(User user)
        {
            _database.SessionDal.EndSession(user.Data.UserId);
        }
    }
}
