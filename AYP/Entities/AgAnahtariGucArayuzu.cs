

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AYP.Entities
{
    public class AgAnahtariGucArayuzu
    {
        [Key]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int AgAnahtariId { get; set; }

        [ForeignKey("AgAnahtariId")]
        public AgAnahtari AgAnahtari { get; set; }

        [Range(1, int.MaxValue)]
        public int GucArayuzuId { get; set; }

        [ForeignKey("GucArayuzuId")]
        public GucArayuzu GucArayuzu { get; set; }

    }
}
