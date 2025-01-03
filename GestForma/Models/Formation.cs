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
        public string? Intitule { get; set; }

        [Required]
        public string? Description { get; set; }


        [Required]
        
        public int? Id_Categorie { get; set; }

        [ForeignKey("Id_Categorie")]
        public Category? Categorie { get; set; }

        [Range(0, float.MaxValue)]
        public float Duree { get; set; }

        public float Cout { get; set; }

        public string? ID_User { get; set; }

    
        [MaxLength(2955)]
        public string FileName { get; set; } // Nom de l'image

        
        [MaxLength(500)]
        public string ContentType { get; set; } // Type MIME (ex. "image/png", "image/jpeg")

        
        public long Size { get; set; } // Taille de l'image en octets

        
        public byte[] Data { get; set; } // Contenu binaire de l'image

        [ForeignKey("ID_User")]
        public ApplicationUser? User { get; set; }

        public virtual ICollection<Inscription>? Inscriptions { get; set; } 
        public virtual ICollection<CommentairesDeFormation>? Commentaires { get; set; }
        public virtual ICollection<Rate>? Evaluations { get; set; } 
    }
}
