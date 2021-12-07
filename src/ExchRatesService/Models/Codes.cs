using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExchRatesService.Models
{
    public class Codes
    {
        public Codes()
        {
            Items = new HashSet<CurrencyCodes>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CurrencyCodes> Items { get; set; }
    }
}
