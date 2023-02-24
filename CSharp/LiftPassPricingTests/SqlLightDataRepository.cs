using System;
using System.Data;
using LiftPassPricing.Application.Ports;
using LiftPassPricing.Domain;
using Microsoft.Data.Sqlite;

namespace LiftPassPricing
{
    public class SqlLightDataRepository : IDataRepository, IDisposable
    {
        private readonly SqliteConnection connection;

        public SqlLightDataRepository()
        {
            string connectionString = new SqliteConnectionStringBuilder()
            {
                Mode = SqliteOpenMode.Memory,
                Cache = SqliteCacheMode.Shared,
                DataSource = "LiftPassPricingInMemory"
            }.ToString();

            this.connection = new SqliteConnection(connectionString);

            this.connection.Open();
        }

        public void Dispose()
        {
            this.connection.Close();
        }

        public void InitDatabase()
        {
            var createCommand = connection.CreateCommand();
            createCommand.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS base_price (
                    type VARCHAR(255) NOT NULL PRIMARY KEY,
                    cost INT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS holidays (
                    holiday DATE NOT NULL,
                    description VARCHAR(255) NOT NULL
                );
            ";

            createCommand.Prepare();
            createCommand.ExecuteNonQuery();

            var insertCommand = connection.CreateCommand();
            insertCommand.CommandText =
            @"
                INSERT INTO base_price (type, cost) VALUES ('1jour', 35);
                INSERT INTO base_price (type, cost) VALUES ('night', 19);

                INSERT INTO holidays (holiday, description) VALUES ('2019-02-18', 'winter');
                INSERT INTO holidays (holiday, description) VALUES ('2019-02-25', 'winter');
                INSERT INTO holidays (holiday, description) VALUES ('2019-03-04', 'winter');
            ";
            insertCommand.Prepare();
            insertCommand.ExecuteNonQuery();
        }

        public void InsertPrice(int basePriceCost, string basePriceType)
        {
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO base_price (type, cost) VALUES (@type, @cost)
                    ON CONFLICT(type) DO UPDATE SET cost = @cost WHERE type = @type;
            ";

            command.Parameters.AddWithValue("@type", basePriceType);
            command.Parameters.AddWithValue("@cost", basePriceCost);
            command.Prepare();
            command.ExecuteNonQuery();
        }

        public bool IsHoliday(DateTime d)
        {
            var holidayCmd = connection.CreateCommand();
            holidayCmd.CommandText =
            @"
                SELECT * FROM holidays
            ";

            holidayCmd.Prepare();
            var holidays = holidayCmd.ExecuteReader();

            while (holidays.Read())
            {
                var holiday = holidays.GetDateTime("holiday");

                if (d.Year == holiday.Year &&
                    d.Month == holiday.Month &&
                    d.Date == holiday.Date)
                {
                    return true;
                }
            }

            return false;
        }

        public int ReadCost(string type)
        {
            var costCmd = connection.CreateCommand();
            costCmd.CommandText =
            @"
                SELECT cost FROM base_price 
                WHERE type = @type
            ";
            costCmd.Parameters.AddWithValue("@type", type);
            costCmd.Prepare();
            var basePrice = costCmd.ExecuteScalar();
            return Convert.ToInt32(basePrice);
        }

        public Price ReadPrice(string liftPassType)
        {
            return new Price(ReadCost(liftPassType));
        }
    }
}
