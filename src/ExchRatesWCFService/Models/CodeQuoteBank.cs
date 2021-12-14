using System.Xml.Serialization;

namespace ExchRatesWCFService.Models
{
    [XmlType(AnonymousType = true)]
    public class CodeQuoteBank
    {
        [XmlAttribute(AttributeName = "ID")]
        public string Id { get; set; }

        [XmlElement(ElementName = "NumCode", IsNullable = true)]
        public string NumCodeStr { get; set; }

        [XmlIgnore]
        public ushort NumCode
        {
            get => !string.IsNullOrWhiteSpace(NumCodeStr)
                   && ushort.TryParse(NumCodeStr, out var value) 
                    ? value : ushort.MinValue;
            set => NumCodeStr = value.ToString();
        }
        public string CharCode { get; set; }

        public uint Nominal { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
