using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

namespace AYP.Entities
{
    public class UcBirim
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

        public byte[] Katalog { get; set; }

        public string KatalogDosyaAdi { get; set; }

        [Required]
        public byte[] Sembol { get; set; }

        [Required]
        public string SembolDosyaAdi { get; set; }

        [Range(1, int.MaxValue)]
        public int UcBirimTurId { get; set; }

        [ForeignKey("UcBirimTurId")]
        public UcBirimTur UcBirimTur { get; set; }

        [Range(0, 5)]
        public int GirdiAgArayuzuSayisi { get; set; }

        [Range(1,5)]
        public int CiktiAgArayuzuSayisi { get; set; }

        [Range(1, 5)]
        public int GucArayuzuSayisi { get; set; }

        public int TipId { get; set; }

        [ForeignKey("TipId")]
        public KL_Tip KL_Tip { get; set; }

        [NotMapped]
        public List<UcBirimTur> UcBirimTurList { get; set; }

        [NotMapped]
        public ImageSource SembolSrc { get; set; }
    }
}
