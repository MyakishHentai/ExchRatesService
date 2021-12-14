using System.Data.Entity;

namespace ExchRatesWCFService.Models.Entity
{
    public class ExchRatesContext : DbContext
    {
        //static ExchRatesContext()
        //{
        //    Database.SetInitializer(new ContextInitializer());
        //}

        public ExchRatesContext() : base("DBModel")
        { }

        public virtual DbSet<CodeQuote> CodeQuotes { get; set; }
        public virtual DbSet<Code> Codes { get; set; }
        public virtual DbSet<Quote> Quotes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Code>()
                .Property(e => e.CharCode)
                .IsFixedLength();

            modelBuilder.Entity<Code>()
                .HasMany(e => e.CodeQuotes)
                .WithRequired(e => e.Code)
                .HasForeignKey(e => e.CodeId);

            modelBuilder.Entity<Quote>()
                .HasMany(e => e.CodeQuotes)
                .WithRequired(e => e.Quote)
                .HasForeignKey(e => e.QuoteId);
        }
    }
}