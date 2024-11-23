using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IDatabase
    {
        IUserDal GetUserDal();
        IBankDetailDal GetBankDetailDal();
        ISessionDal GetSessionDal();

    }
}
