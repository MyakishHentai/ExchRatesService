using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRatesWCFService.Models.Entities
{
    public class QuoteEntity
    {
        public QuoteEntity()
        {
        }

        [Key]
        public int Id { get; set; }
        public string ValuteId { get; set; }

        [DataType(DataType.Date)]
        public string Date { get; set; }

        public string Name { get; set; }
        public float Value { get; set; }

        [ForeignKey("ValuteId")]
        public CurrencyCodesEntity Valute { get; set; }
    }
}
