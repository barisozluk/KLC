

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public decimal GirdiDuraganGerilimDegeri1 { get; set; }

        public decimal GirdiDuraganGerilimDegeri2 { get; set; }

        public decimal GirdiDuraganGerilimDegeri3 { get; set; }

        public decimal GirdiMinimumGerilimDegeri { get; set; }

        public decimal GirdiMaksimumGerilimDegeri { get; set; }

        public decimal GirdiTukettigiGucMiktari { get; set; }

        public string CiktiDuraganGerilimDegeri { get; set; }

        public decimal CiktiUrettigiGucKapasitesi { get; set; }

        [NotMapped]
        public List<string> PortList { get; set; }

        [NotMapped]
        public List<KL_KullanimAmaci> KullanimAmaciList { get; set; }

        [NotMapped]
        public List<KL_GerilimTipi> GerilimTipiList { get; set; }
    }
}
