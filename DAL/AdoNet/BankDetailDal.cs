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
                throw new InvalidOperationException($"Error when getting bank detail data: {ex.Message}");
            }

            return null;
        }

        public void AddBankDetail(int userId)
        {

        }

        public void UpdateBankDetail(BankDetailData data)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = connection.CreateCommand();

                connection.Open();
               
                command.CommandText = @"
                    MERGE INTO BankDetailsTBL AS target
                    USING (SELECT @UserId AS UserId) AS source
                    ON target.UserId = source.UserId
                    WHEN MATCHED THEN
                        UPDATE SET 
                            CardNumber = @CardNumber, 
                            ExpirationDate = @ExpirationDate, 
                            CardCVV = @CardCVV, 
                            CardHolderName = @CardHolderName, 
                            BillingAddress = @BillingAddress
                    WHEN NOT MATCHED THEN
                        INSERT (UserId, CardNumber, ExpirationDate, CardCVV, CardHolderName, BillingAddress)
                        VALUES (@UserId, @CardNumber, @ExpirationDate, @CardCVV, @CardHolderName, @BillingAddress);";

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
                throw new InvalidOperationException($"Error updating bank details: {ex.Message}");
            }
        }

        public List<BankDetailData> GetAllBankDetailData()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using SqlCommand command = connection.CreateCommand();

                command.CommandText = @"SELECT * FROM BankDetailsTBL";
                
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                List<BankDetailData> result = new List<BankDetailData>();

                while (reader.Read())
                {
                    result.Add(new BankDetailData
                    {
                        UserId = reader.GetInt32(0),
                        CardNumber = reader.GetString(1),
                        ExpirationDate = reader.GetString(2),
                        CardCVV = reader.GetString(3),
                        CardHolderName = reader.GetString(4),
                        BillingAddress = reader.GetString(5)
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error when getting bank detail data: {ex.Message}");
            }

            return null;
        }
    }
}