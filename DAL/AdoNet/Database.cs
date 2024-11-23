using DAL.Interface;
using System.Data.SqlClient;

namespace DAL.AdoNet
{
    public class Database : IDatabase
    {
        private readonly string _connectionString;

        private readonly UserDal _user;
        private readonly BankDetailDal _bankDetail;
        private readonly SessionDal _session;

        public IUserDal UserDal => _user;
        public IBankDetailDal BankDetailDal => _bankDetail;
        public ISessionDal SessionDal => _session;

        public Database(string connectionString)
        {
            _connectionString = connectionString;

            _user = new UserDal(_connectionString);
            _bankDetail = new BankDetailDal(_connectionString);
            _session = new SessionDal(_connectionString);
        }
    }
}
