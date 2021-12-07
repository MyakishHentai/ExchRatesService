//using ExchRates.Common.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ExchRatesFrontService.Controllers
//{
//    public class ExchRatesContext : DbContext
//    {
//        public ExchRatesContext(DbContextOptions<ExchRatesContext> options) : base(options)
//        {
//            Database.EnsureCreated();
//        }
//        public DbSet<CodesEntity> Codes { get; set; }
//        public DbSet<QuoteEntity> Quotes { get; set; }

//        public DbSet<CurrencyCodesEntity> Currencies { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//                //optionsBuilder.UseNpgsql("Host=localhost;Database=ExchRates;Username=postgres;Password=localbase");
//            }
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);
//        }
//    }
//}
