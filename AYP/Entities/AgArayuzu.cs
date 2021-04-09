using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AYP.Entities
{
    public class AgArayuzu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Adi { get; set; }

        [Required]
        public string Port { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int KullanimAmaciId { get; set; }

        [ForeignKey("KullanimAmaciId")]
        public KL_KullanimAmaci KL_KullanimAmaci { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int FizikselOrtamId { get; set; }

        [ForeignKey("FizikselOrtamId")]
        public KL_FizikselOrtam KL_FizikselOrtam { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int KapasiteId { get; set; }

        [ForeignKey("KapasiteId")]
        public KL_Kapasite KL_Kapasite { get; set; }

        [Range(1, int.MaxValue)]
        public int? TipId { get; set; }

        [ForeignKey("TipId")]
        public KL_Tip KL_Tip { get; set; }

        [NotMapped]
        public List<KL_Kapasite> KapasiteList { get; set; }

        [NotMapped]
        public List<KL_FizikselOrtam> FizikselOrtamList { get; set; }

        [NotMapped]
        public List<KL_KullanimAmaci> KullanimAmaciList { get; set; }

        [NotMapped]
        public List<string> PortList { get; set; }

        [NotMapped]
        public string Kapasite { get; set; }

        [NotMapped]
        public string KullanimAmaci { get; set; }

        [NotMapped]
        public string FizikselOrtam { get; set; }
    }
}
