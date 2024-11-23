using DAL.Interface;
using DTO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.AdoNet
{
    public class Database : IDatabase
    {
        private readonly SqlConnection connection;

        private readonly User user;
        private readonly BankDetail bankDetail;
        private readonly Session session;

        public Database(string connectionString)
        {
            this.connection = new SqlConnection(connectionString);
            this.user = new User(connection);
            this.bankDetail = new BankDetail(connection);
            this.session = new Session(connection);
        }

        public IBankDetailDal GetBankDetailDal()
        {
            return this.bankDetail;
        }

        public ISessionDal GetSessionDal()
        {
            return this.session;
        }

        public IUserDal GetUserDal()
        {
            return this.user;
        }
    }
}
