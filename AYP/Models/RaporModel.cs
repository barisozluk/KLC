using System;
using System.Collections.Generic;
using System.Text;

namespace AYP.Models
{
    public class RaporModel
    {
        public DateTime Tarih { get; set; }
        public string GizlilikDerecesi { get; set; } = "Tasnif Dışı";
        public string YazimOrtami { get; set; }
        public string Hazirlayan { get; set; }
        public string KontrolEden { get; set; }
        public string Onaylayan { get; set; }
        public string DilKodu { get; set; } = "Türkçe";
        public string DokumanTanimi { get; set; }
        public string Bolum { get; set; }
        public string RevizyonKodu { get; set; }
        public DateTime DegistirmeTarihi { get; set; }
        public string DokumanKodu { get; set; }
        public string DokumanParcaNo { get; set; }
        public string Degistiren { get; set; }
        public string SayfaBoyutu { get; set; } = "A4";
    }
}
