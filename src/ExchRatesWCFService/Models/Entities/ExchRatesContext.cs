using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRatesWCFService.Models.Entities
{
    public class ExchRatesContext : DbContext
    {
        public ExchRatesContext() : base("name=DBConnection")
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<ExchRatesContext>());
            Database.CreateIfNotExists();
        }
        public DbSet<CodesEntity> Codes { get; set; }
        public DbSet<QuoteEntity> Quotes { get; set; }

        public DbSet<CurrencyCodesEntity> Currencies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
}
