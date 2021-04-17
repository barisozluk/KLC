using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AYP.Entities
{
    public class KL_Kapasite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Ad { get; set;  }
        public int MinKapasite { get; set; }
        public int MaxKapasite { get; set; }

    }
}
