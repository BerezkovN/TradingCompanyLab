using DTO;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace DAL.Interface
{
    public interface ISessionDal
    {
        public List<SessionData> GetUserSessions();

        public void StartSession(int userId);
        public void EndSession(int userId);
        public List<SessionData> GetUserSessions(int userId);
    }
}