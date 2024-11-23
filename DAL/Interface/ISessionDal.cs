using DTO;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace DAL.Interface
{
    public interface ISessionDal
    {
        public List<(int UserId, string Username, string Status, DateTime? LoginTime, DateTime? LogoutTime)> GetUserSessions();

        public void StartSession(int userId);
        public void EndSession(int userId);





    }
}