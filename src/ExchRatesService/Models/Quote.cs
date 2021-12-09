using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchRatesService.Models
{
    public class Quote
    {
        public Quote()
        {
            Valutes = new HashSet<QuoteCurrency>();
        }

        [Key]
        [DataType(DataType.Date)]
        public DateTime Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<QuoteCurrency> Valutes { get; set; }
    }
}
