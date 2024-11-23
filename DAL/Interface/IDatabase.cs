namespace DAL.Interface
{
    public interface IDatabase
    {
        public IUserDal UserDal { get; }
        public IBankDetailDal BankDetailDal { get; }
        public ISessionDal SessionDal { get; }

    }
}
