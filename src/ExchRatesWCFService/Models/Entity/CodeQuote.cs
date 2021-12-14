using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchRatesWCFService.Models.Entity
{
    [Table("public.CodeQuotes")]
    public class CodeQuote
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long QuoteId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string CodeId { get; set; }

        public float? Value { get; set; }

        public virtual Code Code { get; set; }

        public virtual Quote Quote { get; set; }
    }
}