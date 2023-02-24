using MySql.Data.MySqlClient;
using System;
using LiftPassPricing.Application.Ports;
using LiftPassPricing.Domain;

namespace LiftPassPricing
{
    public class MySqlDataRepository : IDataRepository, IDisposable
    {
        private readonly MySqlConnection connection;

        public MySqlDataRepository(string connectionInfo)
        {
            this.connection = new MySqlConnection
            {
                ConnectionString = connectionInfo
            };

            this.connection.Open();
        }

        public void Dispose()
        {
            this.connection.Close();
        }

        public void InsertPrice(int basePriceCost, string basePriceType)
        {
            using var command = new MySqlCommand( //
                                   "INSERT INTO base_price (type, cost) VALUES (@type, @cost) " + //
                                   "ON DUPLICATE KEY UPDATE cost = @cost;", this.connection);
            command.Parameters.AddWithValue("@type", basePriceType);
            command.Parameters.AddWithValue("@cost", basePriceCost);
            command.Prepare();
            command.ExecuteNonQuery();
        }

        public bool IsHoliday(DateTime d)
        {
            using (var holidayCmd = new MySqlCommand( //
                                            "SELECT * FROM holidays", connection))
            {
                holidayCmd.Prepare();
                using var holidays = holidayCmd.ExecuteReader();

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
            }

            return false;
        }

        public int ReadCost(string type)
        {
            using var costCmd = new MySqlCommand( //
                    "SELECT cost FROM base_price " + //
                    "WHERE type = @type", this.connection);
            costCmd.Parameters.AddWithValue("@type", type);
            costCmd.Prepare();
            return (int)costCmd.ExecuteScalar();
        }

        public Price ReadPrice(string liftPassType)
        {
            return new Price(ReadCost(liftPassType));
        }
    }
}
