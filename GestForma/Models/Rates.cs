using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestForma.Models
{
    public class Rates
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRate { get; set; }

        [Required]
        public int ID_Formation { get; set; }

        [ForeignKey("ID_Formation")]
        public Formation Formation { get; set; }

        [Required]
        public int ID_User { get; set; }

        [ForeignKey("ID")]
        public ApplicationUser User { get; set; }

        [Required]
        [Range(0, 5)]
        public double Rate { get; set; }
    }
}
