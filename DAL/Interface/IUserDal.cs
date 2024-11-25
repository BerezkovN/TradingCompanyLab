using DTO;

namespace DAL.Interface
{
    public interface IUserDal
    {
        public UserData? Login(string username, string password);

        public void UpdateUser(string columnName, object newValue, int userId);

        public void DeleteUser(int userId);
        public List<UserData> GetAllUsers();
        public UserData GetUser(int userId);
        public UserData? GetUserByUsername(string username);


        public void CreateUser(string username, string email, string password, string recoveryKey);



    }
}