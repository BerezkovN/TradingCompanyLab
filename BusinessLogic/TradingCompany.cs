using DAL.AdoNet;
using DAL.Interface;
using Microsoft.Extensions.Configuration;

namespace BusinessLogic
{
    public class TradingCompany
    {

        private readonly IDatabase _database;

        public IDatabase Database => _database;

        public TradingCompany() 
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();

            string? connectionString = configuration.GetConnectionString("SqlServer");
            if (connectionString == null)
            {
                throw new InvalidOperationException("Не знайдено config.json");
            }

            _database = new Database(connectionString);
        }

    }
}
