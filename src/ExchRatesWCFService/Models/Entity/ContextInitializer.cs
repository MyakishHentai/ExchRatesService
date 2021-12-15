using System.Data.Entity;

namespace ExchRatesWCFService.Models.Entity
{
    internal class ContextInitializer : CreateDatabaseIfNotExists<ExchRatesContext>
    {
        protected override void Seed(ExchRatesContext db)
        {
            //db.SaveChanges();
        }
    }
}