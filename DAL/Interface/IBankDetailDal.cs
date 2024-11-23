using DTO;

namespace DAL.Interface
{
    public interface IBankDetailDal
    {
        public BankDetailData? GetBankDetailData(int userId);
        public void UpdateBankDetail(BankDetailData data);

    }
}