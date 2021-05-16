using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AYP.Models
{
    public class RaporModel
    {
        public DateTime? Tarih { get; set; }

        [Required]
        public string GizlilikDerecesi { get; set; } = "Tasnif Dışı";

        [Required]
        public string YazimOrtami { get; set; }

        [Required]
        public string Hazirlayan { get; set; }

        [Required]
        public string KontrolEden { get; set; }

        [Required]
        public string Onaylayan { get; set; }

        [Required]
        public string DilKodu { get; set; } = "Türkçe";

        [Required]
        public string DokumanTanimi { get; set; }

        [Required]
        public string Bolum { get; set; }

        [Required]
        public string RevizyonKodu { get; set; }
        public DateTime? DegistirmeTarihi { get; set; }

        [Required]
        public string DokumanKodu { get; set; }

        [Required]
        public string DokumanParcaNo { get; set; }

        [Required]
        public string Degistiren { get; set; }

        [Required]
        public string SayfaBoyutu { get; set; } = "A4";
    }
}
