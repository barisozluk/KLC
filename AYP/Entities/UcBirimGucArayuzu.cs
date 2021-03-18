

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AYP.Entities
{
    public class UcBirimGucArayuzu
    {
        [Key]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int UcBirimId { get; set; }

        [ForeignKey("UcBirimId")]
        public UcBirim UcBirim { get; set; }

        [Range(1, int.MaxValue)]
        public int GucArayuzuId { get; set; }

        [ForeignKey("GucArayuzuId")]
        public GucArayuzu GucArayuzu { get; set; }

    }
}
