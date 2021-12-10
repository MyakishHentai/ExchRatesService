using System.Xml.Serialization;

namespace ExchRatesWCFService.Models
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false, ElementName = "ValCurs")]
    public class QuoteBank
    {
        [XmlElement("Valute")]
        public CodeQuoteBank[] Valutes { get; set; }


        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }


        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }
}
