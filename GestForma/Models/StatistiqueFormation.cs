using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestForma.Models
{
    public class StatistiqueFormation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdStatistiqueFormation { get; set; }

        [Required]
        [ForeignKey("Formation")]
        public int IdFormation { get; set; }

        [Required]
        public int ParticipantsParFormation { get; set; }

        [Required]
        [Range(0, 5)]
        public double RateMoyen { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }

        public virtual Formation? Formation { get; set; }
    }
}
