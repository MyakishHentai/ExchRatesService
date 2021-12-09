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
        public DbSet<QuoteCurrency> QuotesCurrencies { get; set; }

        

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

            modelBuilder.Entity<QuoteCurrency>(entity =>
            {
                entity.HasKey(e => new { e.QuoteId, e.CodeId })
                    .HasName("PK_QuoteCurrency_ManyToMany");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Quote)
                    .WithMany(p => p.Valutes)
                    .HasForeignKey(d => d.QuoteId)
                    .HasConstraintName("FK_QuoteCurrency_Quote");

                entity.HasOne(d => d.Code)
                    .WithMany(p => p.Quotes)
                    .HasForeignKey(d => d.CodeId)
                    .HasConstraintName("FK_QuoteCurrency_Code");
            });
        }
    }
}
