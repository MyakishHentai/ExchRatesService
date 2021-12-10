using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ExchRatesWCFService.Models.Entity
{
    public partial class ExchRatesContext : DbContext
    {
        //static ExchRatesContext()
        //{
        //    Database.SetInitializer(new ContextInitializer());
        //}

        public ExchRatesContext()
            : base(nameOrConnectionString:"DBModel")
        {}

        public virtual DbSet<CodeQuote> CodeQuotes { get; set; }
        public virtual DbSet<Code> Codes { get; set; }
        public virtual DbSet<Quote> Quotes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CodeQuote>()
                .Property(e => e.CodeId)
                .IsFixedLength();

            modelBuilder.Entity<Code>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<Code>()
                .Property(e => e.ParentCode)
                .IsFixedLength();

            modelBuilder.Entity<Code>()
                .Property(e => e.CharCode)
                .IsFixedLength();

            modelBuilder.Entity<Code>()
                .HasMany(e => e.CodeQuotes)
                .WithRequired(e => e.Codes)
                .HasForeignKey(e => e.CodeId);

            modelBuilder.Entity<Quote>()
                .HasMany(e => e.CodeQuotes)
                .WithRequired(e => e.Quotes)
                .HasForeignKey(e => e.QuoteId);
        }
    }
}
