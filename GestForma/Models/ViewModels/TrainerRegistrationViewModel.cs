using System.ComponentModel.DataAnnotations;

namespace GestForma.Models.ViewModels
{
    public class TrainerRegistrationViewModel
    {
        // Champs provenant de ApplicationUser
        [Required]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; } = "";

        [Required]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = "";

        public string Address { get; set; } = "";
        [Required]
        public string PhoneNumber { get; set; } = "";

        // Champs spécifiques à Trainer
        [Required]
        public string Field { get; set; } = "";

        public IFormFile? ProfileImage { get; set; }
    }
}
