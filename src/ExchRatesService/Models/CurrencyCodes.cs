using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchRatesService.Models
{
    public class CurrencyCodes
    {
        public CurrencyCodes()
        {
            Quotes = new HashSet<QuoteCurrency>();
        }

        [Key]
        public string Id { get; set; }
        public int? MarketId { get; set; }

        public string Name { get; set; }

        public string EngName { get; set; }

        public uint Nominal { get; set; }

        public string ParentCode { get; set; }
        public ushort NumCode { get; set; }
        public string CharCode { get; set; }

        [ForeignKey("MarketId")]
        public Codes Market { get; set; }
        public virtual ICollection<QuoteCurrency> Quotes { get; set; }

    }
}
