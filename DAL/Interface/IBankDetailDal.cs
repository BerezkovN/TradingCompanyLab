using DTO;

namespace DAL.Interface
{
    public interface IBankDetailDal
    {
        List<BankDetailData> GetAllBankDetailData();
        public BankDetailData? GetBankDetailData(int userId);
        public void UpdateBankDetail(BankDetailData data);

    }
}