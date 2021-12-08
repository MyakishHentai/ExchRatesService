using System.Xml.Serialization;

namespace ExchRatesWCFService.Models
{
    [XmlType(AnonymousType = true)]
    public class CurrencyCodesDesc
    {
        [XmlAttribute(AttributeName = "ID")]
        public string Id { get; set; }

        public string Name { get; set; }

        public string EngName { get; set; }

        public uint Nominal { get; set; }

        public string ParentCode { get; set; }


        [XmlElement(ElementName = "ISO_Num_Code", IsNullable = true)]
        public string NumCodeStr { get; set; }

        [XmlIgnore]
        public ushort NumCode
        {
            get
            {
                return !string.IsNullOrWhiteSpace(NumCodeStr)
                        && ushort.TryParse(NumCodeStr, out ushort value) ?
                        value : ushort.MinValue;
            }
            set
            {
                NumCodeStr = value.ToString();
            }
        }

        [XmlElement(ElementName = "ISO_Char_Code", IsNullable = true)]
        public string CharCode { get; set; }
    }
}
