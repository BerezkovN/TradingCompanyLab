namespace DTO
{
    public class BankDetailData
    {
        public int UserId { get; set; }

        public string? CardNumber { get; set; }

        public string? ExpirationDate { get; set; }

        public string? CardCVV { get; set; }

        public string? CardHolderName { get; set; }

        public string? BillingAddress { get; set; }


        public static bool IsValidCardNumber(string? cardNumber)
        {
            if (cardNumber == null) return false;

            if (cardNumber.Length < 10 || cardNumber.Length > 16)
                return false;


            for (int ind = 0; ind < cardNumber.Length; ind++)
            {
                if (!char.IsDigit(cardNumber[ind]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsValidCVV(string? cvv)
        {
            if (cvv == null) 
                return false;

            if (string.IsNullOrWhiteSpace(cvv) || (cvv.Length != 3 && cvv.Length != 4))
                return false;

            return cvv.All(char.IsDigit);
        }

        public static bool IsValidExpirationDate(string? expirationDate)
        {
            if (expirationDate == null) 
                return false;

            if (string.IsNullOrWhiteSpace(expirationDate) || !expirationDate.Contains("/"))
                return false;

            string[] parts = expirationDate.Split('/');
            if (parts.Length != 2 || !int.TryParse(parts[0], out int month) || !int.TryParse(parts[1], out int year))
                return false;

            if (month < 1 || month > 12)
                return false;

            if (year < 100)
                year += 2000;

            DateTime cardExpiry = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            return cardExpiry >= DateTime.Now;
        }
    }
}
