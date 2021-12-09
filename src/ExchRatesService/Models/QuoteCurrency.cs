using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRatesService.Models
{
    public class QuoteCurrency
    {
        [DataType(DataType.Date)]
        public DateTime QuoteId { get; set; }
        public string CodeId { get; set; }
        public float? Value { get; set; }
        public virtual Quote Quote { get; set; }
        public virtual CurrencyCodes Code { get; set; }
    }
}
