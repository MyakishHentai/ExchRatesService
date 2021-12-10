using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace ExchRatesWCFService.Models.Entity
{
    [Table("public.CodeQuotes")]
    public partial class CodeQuote
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long QuoteId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(12)]
        public string CodeId { get; set; }

        public float? Value { get; set; }

        public virtual Code Codes { get; set; }

        public virtual Quote Quotes { get; set; }
    }
}
