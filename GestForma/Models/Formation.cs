using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestForma.Models
{
    public class Formation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Formation { get; set; }

        [Required]
        [MaxLength(255)]
        public string Intitule { get; set; }

        [Required]
        public string Description { get; set; }

        [MaxLength(100)]
        public string Categorie { get; set; }

        public float Duree { get; set; }

        public float Cout { get; set; }

        public int? ID_User { get; set; }

        [ForeignKey("Id")]
        public ApplicationUser User { get; set; }

        public ICollection<Inscription> Inscriptions { get; set; }

        public ICollection<CommentairesDeFormation> Commentaires { get; set; }

        public ICollection<Rates> Evaluations { get; set; }
    }
}
