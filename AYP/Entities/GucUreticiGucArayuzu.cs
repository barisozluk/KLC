﻿

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AYP.Entities
{
    public class GucUreticiGucArayuzu
    {
        [Key]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int GucUreticiId { get; set; }

        [ForeignKey("GucUreticiId")]
        public GucUretici GucUretici { get; set; }

        [Range(1, int.MaxValue)]
        public int GucArayuzuId { get; set; }

        [ForeignKey("GucArayuzuId")]
        public GucArayuzu GucArayuzu { get; set; }

    }
}
