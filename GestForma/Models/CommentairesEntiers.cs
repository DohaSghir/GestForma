using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestForma.Models
{
    public class CommentairesEntiers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCommentaire { get; set; }

        [Required]
        [MaxLength(2000)]
        public string? ContenuCommentaire { get; set; }
    }
}
