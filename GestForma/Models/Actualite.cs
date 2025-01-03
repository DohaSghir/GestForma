using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestForma.Models
{
    public class Actualite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdActualite { get; set; }

        [Required]
        [MaxLength(100)]
        public string Titre { get; set; }
        [Required]
        public string Description { get; set; }

    }
}
