using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AYP.Entities
{
    public class KL_GerilimTipi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Ad { get; set;  }
        
    }
}
