using AYP.Models;
using AYP.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AYP.Entities
{
    public class AgAkis: ICloneable
    {
        public Guid Id { get; set; }
        public Guid AgArayuzuId { get; set; }
        public decimal Yuk { get; set; }
        public int AgAkisTipiId { get; set; }
        public string AgAkisTipiAdi { get; set; }
        public List<KeyValuePair<Guid, string>> VarisNoktasiIdNameList { get; set; }
        public Guid? IliskiliAgArayuzuId { get; set; }
        public string IliskiliAgArayuzuAdi { get; set; }
        public Guid? IliskiliAgArayuzuAgAkisId { get; set; }
        public Guid? FromNodeUniqueId { get; set; }
        public string FromNodeName { get; set; }
        public List<KodListModel> AgAkisTipiList { get; set; }
        public List<NodeViewModel> VarisNoktasiList { get; set; }
        public List<ConnectorViewModel> InputList { get; set; }

        public XElement ToXElement()
        {
            XElement element = new XElement("AgAkis");
            element.Add(new XAttribute("UniqueId", Id));
            element.Add(new XAttribute("AgArayuzuUniqueId", AgArayuzuId));
            element.Add(new XAttribute("Yuk", Yuk));
            element.Add(new XAttribute("AgAkisTipiId", AgAkisTipiId));
            element.Add(!IliskiliAgArayuzuAgAkisId.HasValue ? null : new XAttribute("IliskiliAgArayuzuAgAkisId", IliskiliAgArayuzuAgAkisId));
            element.Add(!IliskiliAgArayuzuId.HasValue ? null : new XAttribute("IliskiliAgArayuzuUniqueId", IliskiliAgArayuzuId));
            element.Add(!FromNodeUniqueId.HasValue ? null : new XAttribute("FromNodeUniqueId", FromNodeUniqueId));

            return element;
        }

        public object Clone()
        {
            var result = (AgAkis)this.MemberwiseClone();
            return result;
        }
    }
}
