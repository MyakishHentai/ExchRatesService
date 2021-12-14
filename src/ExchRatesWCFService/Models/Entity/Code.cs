using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace ExchRatesWCFService.Models.Entity
{
    [Table("public.Codes")]
    public partial class Code
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Code()
        {
            CodeQuotes = new HashSet<CodeQuote>();
        }

        [StringLength(12)]
        public string Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string EngName { get; set; }

        public int Nominal { get; set; }

        [StringLength(12)]
        public string ParentCode { get; set; }

        public short? NumCode { get; set; }

        [StringLength(3)]
        public string CharCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CodeQuote> CodeQuotes { get; set; }
    }
}
