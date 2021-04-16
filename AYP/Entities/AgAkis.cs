using AYP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AYP.Entities
{
    public class AgAkis
    {
        public Guid Id { get; set; }
        public Guid AgArayuzuId { get; set; }
        public decimal Yuk { get; set; }
        public int AgAkisTipiId { get; set; }
        public string AgAkisTipiAdi { get; set; }
        public int AgAkisProtokoluId { get; set; }
        public string AgAkisProtokoluAdi { get; set; }
        public int? IliskiliAgArayuzuId { get; set; }
        public List<KodListModel> AgAkisProtokoluList { get; set; }
        public List<KodListModel> AgAkisTipiList { get; set; }

    }
}
