using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ExchRatesWCFService.Models.Entity
{
    [Table("public.Codes")]
    public class Code
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Code()
        {
            CodeQuotes = new HashSet<CodeQuote>();
        }

        [StringLength(8)] public string Id { get; set; }

        [Required] [StringLength(255)] public string Name { get; set; }

        [StringLength(255)] public string EngName { get; set; }

        public int Nominal { get; set; }

        [StringLength(12)] public string ParentCode { get; set; }

        public short? NumCode { get; set; }

        [StringLength(3)] public string CharCode { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<CodeQuote> CodeQuotes { get; set; }
    }
}