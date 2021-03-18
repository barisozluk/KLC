﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

namespace AYP.Entities
{
    public class AgAnahtari
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
        public byte[] Sembol { get; set; }

        [Range(1, int.MaxValue)]
        public int AgAnahtariTurId { get; set; }

        [ForeignKey("AgAnahtariTurId")]
        public AgAnahtariTur AgAnahtariTur { get; set; }

        [Range(1, int.MaxValue)]
        public int GirdiAgArayuzuSayisi { get; set; }

        [Range(1, int.MaxValue)]
        public int CiktiAgArayuzuSayisi { get; set; }

        [Range(1, int.MaxValue)]
        public int GucArayuzuSayisi { get; set; }

        public int TipId { get; set; }

        [ForeignKey("TipId")]
        public KL_Tip KL_Tip { get; set; }

        [NotMapped]
        public List<AgAnahtariTur> AgAnahtariTurList { get; set; }

        [NotMapped]
        public ImageSource SembolSrc { get; set; }
    }
}