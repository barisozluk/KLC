

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace AYP.Entities
{
    public class GucArayuzu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Adi { get; set; }

        [Required]
        public string Port { get; set; }

        [Range(1, int.MaxValue)]
        public int KullanimAmaciId { get; set; }

        [ForeignKey("KullanimAmaciId")]
        public KL_KullanimAmaci KL_KullanimAmaci { get; set; }

        [Range(1, int.MaxValue)]
        public int GerilimTipiId { get; set; }

        [ForeignKey("GerilimTipiId")]
        public KL_GerilimTipi KL_GerilimTipi { get; set; }

        public int? TipId { get; set; }

        [ForeignKey("TipId")]
        public KL_Tip KL_Tip { get; set; }

        public decimal? GirdiDuraganGerilimDegeri1 { get; set; }

        public decimal? GirdiDuraganGerilimDegeri2 { get; set; }

        public decimal? GirdiDuraganGerilimDegeri3 { get; set; }

        public decimal? GirdiMinimumGerilimDegeri { get; set; }

        public decimal? GirdiMaksimumGerilimDegeri { get; set; }

        public decimal? GirdiTukettigiGucMiktari { get; set; }
        [Range(-1,double.MaxValue)]
        public decimal? CiktiDuraganGerilimDegeri { get; set; }

        public decimal? CiktiUrettigiGucKapasitesi { get; set; }

        [NotMapped]
        public List<string> PortList { get; set; }

        [NotMapped]
        public List<KL_KullanimAmaci> KullanimAmaciList { get; set; }

        [NotMapped]
        public List<KL_GerilimTipi> GerilimTipiList { get; set; }

        public XElement ToXElement(Guid NodeUniqueId)
        {
            XElement element = new XElement("GucArayuzu");
            element.Add(new XAttribute("Id", Id));
            element.Add(new XAttribute("Adi", Adi));
            element.Add(new XAttribute("Port", Port));
            element.Add(new XAttribute("KullanimAmaciId", KullanimAmaciId));
            element.Add(new XAttribute("GerilimTipiId", GerilimTipiId));
            element.Add(new XAttribute("TipId", TipId));
            element.Add(!GirdiDuraganGerilimDegeri1.HasValue ? null : new XAttribute("GirdiDuraganGerilimDegeri1", GirdiDuraganGerilimDegeri1));
            element.Add(!GirdiDuraganGerilimDegeri2.HasValue ? null : new XAttribute("GirdiDuraganGerilimDegeri2", GirdiDuraganGerilimDegeri2));
            element.Add(!GirdiDuraganGerilimDegeri3.HasValue ? null : new XAttribute("GirdiDuraganGerilimDegeri3", GirdiDuraganGerilimDegeri3));
            element.Add(!GirdiMinimumGerilimDegeri.HasValue ? null : new XAttribute("GirdiMinimumGerilimDegeri", GirdiMinimumGerilimDegeri));
            element.Add(!GirdiMaksimumGerilimDegeri.HasValue ? null : new XAttribute("GirdiMaksimumGerilimDegeri", GirdiMaksimumGerilimDegeri));
            element.Add(!GirdiTukettigiGucMiktari.HasValue ? null : new XAttribute("GirdiTukettigiGucMiktari", GirdiTukettigiGucMiktari));
            element.Add(CiktiDuraganGerilimDegeri == null ? null : new XAttribute("CiktiDuraganGerilimDegeri", CiktiDuraganGerilimDegeri));
            element.Add(!CiktiUrettigiGucKapasitesi.HasValue ? null : new XAttribute("CiktiUrettigiGucKapasitesi", CiktiUrettigiGucKapasitesi));
            element.Add(new XAttribute("NodeUniqueId", NodeUniqueId));

            return element;
        }

        public XElement ToGroupXElement(Guid GroupId)
        {
            XElement element = new XElement("GroupGucArayuzu");
            element.Add(new XAttribute("Id", Id));
            element.Add(new XAttribute("Adi", Adi));
            element.Add(new XAttribute("Port", Port));
            element.Add(new XAttribute("KullanimAmaciId", KullanimAmaciId));
            element.Add(new XAttribute("GerilimTipiId", GerilimTipiId));
            element.Add(new XAttribute("TipId", TipId));
            element.Add(!GirdiDuraganGerilimDegeri1.HasValue ? null : new XAttribute("GirdiDuraganGerilimDegeri1", GirdiDuraganGerilimDegeri1));
            element.Add(!GirdiDuraganGerilimDegeri2.HasValue ? null : new XAttribute("GirdiDuraganGerilimDegeri2", GirdiDuraganGerilimDegeri2));
            element.Add(!GirdiDuraganGerilimDegeri3.HasValue ? null : new XAttribute("GirdiDuraganGerilimDegeri3", GirdiDuraganGerilimDegeri3));
            element.Add(!GirdiMinimumGerilimDegeri.HasValue ? null : new XAttribute("GirdiMinimumGerilimDegeri", GirdiMinimumGerilimDegeri));
            element.Add(!GirdiMaksimumGerilimDegeri.HasValue ? null : new XAttribute("GirdiMaksimumGerilimDegeri", GirdiMaksimumGerilimDegeri));
            element.Add(!GirdiTukettigiGucMiktari.HasValue ? null : new XAttribute("GirdiTukettigiGucMiktari", GirdiTukettigiGucMiktari));
            element.Add(CiktiDuraganGerilimDegeri == null ? null : new XAttribute("CiktiDuraganGerilimDegeri", CiktiDuraganGerilimDegeri));
            element.Add(!CiktiUrettigiGucKapasitesi.HasValue ? null : new XAttribute("CiktiUrettigiGucKapasitesi", CiktiUrettigiGucKapasitesi));
            element.Add(new XAttribute("GroupId", GroupId));

            return element;
        }
    }
}
