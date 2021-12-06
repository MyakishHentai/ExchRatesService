using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRatesWCFService.Models.Entities
{
    public class CurrencyCodesEntity
    {
        public CurrencyCodesEntity()
        {
            Quotes = new HashSet<QuoteEntity>();
        }

        [Key]        
        public string Id { get; set; }
        public int MarketId { get; set; }

        public string Name { get; set; }

        public string EngName { get; set; }

        public uint Nominal { get; set; }

        public string ParentCode { get; set; }
        [Index(IsUnique = true)]
        public ushort NumCode { get; set; }
        [Index(IsUnique = true)]
        public string CharCode { get; set; }

        [ForeignKey("MarketId")]
        public CodesEntity Market { get; set; }
        public ICollection<QuoteEntity> Quotes { get; set; }

    }
}
