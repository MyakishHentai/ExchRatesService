using System.Data.Entity;

namespace ExchRatesWCFService.Models.Entities
{
    public class ExchRatesContext : DbContext
    {
        public ExchRatesContext() : base(nameOrConnectionString: "DBConnection")
        {
            Database.SetInitializer<ExchRatesContext>(null);
            Database.SetInitializer(new CreateDatabaseIfNotExists<ExchRatesContext>());
            Database.SetInitializer(new ExchRatesDBInitializer());
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
