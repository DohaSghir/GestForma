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
        [Display(Name = "Title")]
        public string? Intitule { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Category")]
        public string? Categorie { get; set; }

        [Range(0, float.MaxValue)]
        [Display(Name = "Duration")]
        public float Duree { get; set; }

        [Display(Name = "Cost")]
        public float Cout { get; set; }

        
        public string? ID_User { get; set; }

        [ForeignKey("ID_User")]
        public ApplicationUser? User { get; set; }

        public virtual ICollection<Inscription>? Inscriptions { get; set; } 
        public virtual ICollection<CommentairesDeFormation>? Commentaires { get; set; }
        public virtual ICollection<Rate>? Evaluations { get; set; } 
    }
}
