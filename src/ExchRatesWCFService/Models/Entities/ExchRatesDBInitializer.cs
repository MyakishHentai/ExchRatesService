using System.Data.Entity;

namespace ExchRatesWCFService.Models.Entities
{
    public class ExchRatesDBInitializer : DropCreateDatabaseAlways<ExchRatesContext>
    {
        protected override void Seed(ExchRatesContext context)
        {
            // TODO:
            //context.Codes.AddRange();

            base.Seed(context);
        }
    }
}
