using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Linq;

namespace AYP.Entities
{
    public class AgArayuzu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Adi { get; set; }

        [Required]
        public string Port { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int KullanimAmaciId { get; set; }

        [ForeignKey("KullanimAmaciId")]
        public KL_KullanimAmaci KL_KullanimAmaci { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int FizikselOrtamId { get; set; }

        [ForeignKey("FizikselOrtamId")]
        public KL_FizikselOrtam KL_FizikselOrtam { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int KapasiteId { get; set; }

        [ForeignKey("KapasiteId")]
        public KL_Kapasite KL_Kapasite { get; set; }

        [Range(1, int.MaxValue)]
        public int? TipId { get; set; }

        [ForeignKey("TipId")]
        public KL_Tip KL_Tip { get; set; }

        [NotMapped]
        public List<KL_Kapasite> KapasiteList { get; set; }

        [NotMapped]
        public List<KL_FizikselOrtam> FizikselOrtamList { get; set; }

        [NotMapped]
        public List<KL_KullanimAmaci> KullanimAmaciList { get; set; }

        [NotMapped]
        public List<string> PortList { get; set; }

        public XElement ToXElement(Guid NodeUniqueId)
        {
            XElement element = new XElement("AgArayuzu");
            element.Add(new XAttribute("Id", Id));
            element.Add(new XAttribute("Adi", Adi));
            element.Add(new XAttribute("Port", Port));
            element.Add(new XAttribute("KullanimAmaciId", KullanimAmaciId));
            element.Add(new XAttribute("FizikselOrtamId", FizikselOrtamId));
            element.Add(new XAttribute("KapasiteId", KapasiteId));
            element.Add(new XAttribute("TipId", TipId));
            element.Add(new XAttribute("NodeUniqueId", NodeUniqueId));

            return element;
        }

        public XElement ToGroupXElement(Guid GroupUniqueId)
        {
            XElement element = new XElement("GroupAgArayuzu");
            element.Add(new XAttribute("Id", Id));
            element.Add(new XAttribute("Adi", Adi));
            element.Add(new XAttribute("Port", Port));
            element.Add(new XAttribute("KullanimAmaciId", KullanimAmaciId));
            element.Add(new XAttribute("FizikselOrtamId", FizikselOrtamId));
            element.Add(new XAttribute("KapasiteId", KapasiteId));
            element.Add(new XAttribute("TipId", TipId));
            element.Add(new XAttribute("GroupUniqueId", GroupUniqueId));

            return element;
        }
    }
}
