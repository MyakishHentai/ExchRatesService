using Microsoft.EntityFrameworkCore;

namespace ExchRatesService.Models
{
    public class ExchRatesContext : DbContext
    {
        public ExchRatesContext()
        {
        }

        public ExchRatesContext(DbContextOptions<ExchRatesContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Codes> Codes { get; set; }
        public DbSet<Quote> Quotes { get; set; }

        public DbSet<CurrencyCodes> Currencies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=my_host;Database=my_db;Username=my_user;Password=my_pw");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
