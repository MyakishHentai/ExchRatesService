using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExchRatesWCFService.Models
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false, ElementName = "Valuta")]
    public class CodesDesc
    {
        [XmlElement("Item")]
        public CurrencyCodesDesc[] Items { get; set; }


        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }
}
