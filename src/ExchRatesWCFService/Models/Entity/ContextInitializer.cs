using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRatesWCFService.Models.Entity
{
    class ContextInitializer : DropCreateDatabaseIfModelChanges<ExchRatesContext>
    {
        protected override void Seed(ExchRatesContext db)
        {
            //db.SaveChanges();
        }
    }
}
