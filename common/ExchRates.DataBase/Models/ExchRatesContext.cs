using ExchRates.DataBase.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRates.DataBaseModels
{
    public class ExchRatesContext : DbContext
    {
        private readonly string _connection;
        public ExchRatesContext(string connection)
        {
            _connection = connection;
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(_connection);
        public DbSet<CodesEntity> Codes { get; set; }
        public DbSet<QuoteEntity> Quotes { get; set; }

        public DbSet<CurrencyCodesEntity> Currencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
}
