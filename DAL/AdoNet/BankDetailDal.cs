using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL.Interface;
using DTO;

namespace DAL.AdoNet
{
    public class BankDetailDal : IBankDetailDal
    {
        private readonly string _connectionString;

        public BankDetailDal(string connection)
        {
            _connectionString = connection;
        }

        public BankDetailData? GetBankDetailData(int userId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using SqlCommand command = connection.CreateCommand();
                
                command.CommandText = 
                    @"SELECT CardNumber, ExpirationDate, CardCVV, CardHolderName, BillingAddress
                      FROM BankDetailsTBL 
                      WHERE UserId = @UserId";
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                
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
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при отриманні банківських даних: {ex.Message}");
                throw;
            }

            return null;
        }

       
        public void UpdateBankDetail(BankDetailData data)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = connection.CreateCommand();
                
                command.CommandText = 
                    @"UPDATE BankDetailsTBL 
                      SET CardNumber = @CardNumber, ExpirationDate = @ExpirationDate, CardCVV = @CardCVV, CardHolderName = @CardHolderName, BillingAddress = @BillingAddress
                      WHERE UserId = @UserId";

                command.Parameters.AddWithValue("@UserId", data.UserId);
                command.Parameters.AddWithValue("@CardNumber", data.CardNumber ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ExpirationDate", data.ExpirationDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CardCVV", data.CardCVV ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CardHolderName", data.CardHolderName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@BillingAddress", data.BillingAddress ?? (object)DBNull.Value);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при оновленні банківських даних: {ex.Message}");
                throw;
            }
        }
       
    }
}
