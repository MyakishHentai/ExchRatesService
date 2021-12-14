using System.Data.Entity;
using Npgsql;

namespace ExchRatesWCFService.Models.Entity
{
    public class NpgSqlConfiguration : DbConfiguration
    {
        private const string NAME = "Npgsql";

        public NpgSqlConfiguration()
        {
            SetProviderFactory(NAME,
                NpgsqlFactory.Instance);

            SetProviderServices(NAME,
                NpgsqlServices.Instance);

            SetDefaultConnectionFactory(new NpgsqlConnectionFactory());
        }
    }
}