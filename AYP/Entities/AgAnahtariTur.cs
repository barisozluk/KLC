using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AYP.Entities
{
    public class AgAnahtariTur
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Ad { get; set; }
    }
}
