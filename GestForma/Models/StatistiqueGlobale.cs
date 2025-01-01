using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestForma.Models
{
    public class StatistiqueGlobale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdStatistiqueGlobale { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }

        [Required]
        public int NombreDeFormations { get; set; }

        [Required]
        public int NombreDeParticipants { get; set; }

        [Required]
        public int NombreDeCategories { get; set; }


    }

}
