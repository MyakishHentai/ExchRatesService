namespace ExchRatesWCFService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.CodeQuotes",
                c => new
                    {
                        QuoteId = c.Int(nullable: false),
                        CodeId = c.String(nullable: false, maxLength: 128),
                        Value = c.Single(),
                    })
                .PrimaryKey(t => new { t.QuoteId, t.CodeId })
                .ForeignKey("public.Codes", t => t.CodeId, cascadeDelete: true)
                .ForeignKey("public.Quotes", t => t.QuoteId, cascadeDelete: true)
                .Index(t => t.QuoteId)
                .Index(t => t.CodeId);
            
            CreateTable(
                "public.Codes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.Quotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.CodeQuotes", "QuoteId", "public.Quotes");
            DropForeignKey("public.CodeQuotes", "CodeId", "public.Codes");
            DropIndex("public.CodeQuotes", new[] { "CodeId" });
            DropIndex("public.CodeQuotes", new[] { "QuoteId" });
            DropTable("public.Quotes");
            DropTable("public.Codes");
            DropTable("public.CodeQuotes");
        }
    }
}
