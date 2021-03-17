
using System.ComponentModel.DataAnnotations;

namespace AYP.Entities
{
    public class UcBirimTur
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Ad { get; set; }
    }
}
