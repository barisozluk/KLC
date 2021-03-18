using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AYP.Entities
{
    public class AgAnahtariAgArayuzu
    {
        [Key]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int AgArayuzuId { get; set; }

        [ForeignKey("AgArayuzuId")]
        public AgArayuzu AgArayuzu { get; set; }

        [Range(1, int.MaxValue)]
        public int AgAnahtariId { get; set; }

        [ForeignKey("AgAnahtariId")]
        public AgAnahtari AgAnahtari { get; set; }
    }
}
