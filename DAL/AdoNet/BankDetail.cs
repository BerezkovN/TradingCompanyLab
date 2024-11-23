using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Interface;
using DTO;

namespace DAL.AdoNet
{
    public class BankDetail : IBankDetailDal
    {
        private readonly SqlConnection _connection;

        public BankDetail(SqlConnection connection)
        {
            _connection = connection;
        }

        public BankDetailData GetBankDetailData(int userId)
        {
            try
            {
                using (SqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = @"SELECT CardNumber, ExpirationDate, CardCVV, CardHolderName, BillingAddress
                                          FROM BankDetailsTBL 
                                          WHERE UserId = @UserId";
                    command.Parameters.AddWithValue("@UserId", userId);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new BankDetailData
                            {
                                UserId = userId,
                                CardNumber = reader["CardNumber"] as string,
                                ExpirationDate = reader["ExpirationDate"] as string,
                                CardCVV = reader["CardCVV"] as string,
                                CardHolderName = reader["CardHolderName"] as string,
                                BillingAddress = reader["BillingAddress"] as string
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при отриманні банківських даних: {ex.Message}");
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return null;
        }

       
        public void UpdateBankDetail(BankDetailData data)
        {
            try
            {
                using (SqlCommand command = _connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE BankDetailsTBL 
                                              SET CardNumber = @CardNumber, ExpirationDate = @ExpirationDate, CardCVV = @CardCVV, CardHolderName = @CardHolderName, BillingAddress = @BillingAddress
                                              WHERE UserId = @UserId";

                    command.Parameters.AddWithValue("@UserId", data.UserId);
                    command.Parameters.AddWithValue("@CardNumber", data.CardNumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ExpirationDate", data.ExpirationDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CardCVV", data.CardCVV ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CardHolderName", data.CardHolderName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@BillingAddress", data.BillingAddress ?? (object)DBNull.Value);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при оновленні банківських даних: {ex.Message}");
                throw;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }
        
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
