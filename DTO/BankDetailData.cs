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


        public static bool IsValidCardNumber(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 13 || cardNumber.Length > 19)
                return false;

            // Перевірка алгоритмом Луна
            int sum = 0;
            bool alternate = false;
            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(cardNumber[i]))
                    return false;

                int digit = cardNumber[i] - '0';
                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }
                sum += digit;
                alternate = !alternate;
            }
            return sum % 10 == 0;
        }

        public static bool IsValidCVV(string cvv)
        {
            if (string.IsNullOrWhiteSpace(cvv) || (cvv.Length != 3 && cvv.Length != 4))
                return false;

            return cvv.All(char.IsDigit);
        }

        public static bool IsValidExpirationDate(string expirationDate)
        {
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
