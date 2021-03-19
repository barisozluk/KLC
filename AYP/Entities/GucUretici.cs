using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

namespace AYP.Entities
{
    public class GucUretici
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string StokNo { get; set; }

        [Required]
        [StringLength(250)]
        public string Tanim { get; set; }

        [Required]
        [StringLength(250)]
        public string UreticiAdi { get; set; }

        [Required]
        [StringLength(50)]
        public string UreticiParcaNo { get; set; }

        [Required]
        public byte[] Katalog { get; set; }

        [Required]
        public string KatalogDosyaAdi { get; set; }

        [Required]
        public byte[] Sembol { get; set; }

        [Required]
        public string SembolDosyaAdi { get; set; }

        [Range(1, int.MaxValue)]
        public int GucUreticiTurId { get; set; }

        [ForeignKey("GucUreticiTurId")]
        public GucUreticiTur GucUreticiTur { get; set; }

        [Range(1, int.MaxValue)]
        public int GirdiGucArayuzuSayisi { get; set; }

        [Range(1, int.MaxValue)]
        public int CiktiGucArayuzuSayisi { get; set; }

        public decimal? VerimlilikDegeri { get; set; }

        public decimal? DahiliGucTuketimDegeri { get; set; }

        public int TipId { get; set; }

        [ForeignKey("TipId")]
        public KL_Tip KL_Tip { get; set; }

        [NotMapped]
        public List<GucUreticiTur> GucUreticiTurList { get; set; }

        [NotMapped]
        public ImageSource SembolSrc { get; set; }
    }
}
