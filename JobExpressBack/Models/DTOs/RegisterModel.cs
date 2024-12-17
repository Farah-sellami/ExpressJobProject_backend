using JobExpressBack.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace JobExpressBack.Models.DTOs
{
    public class RegisterModel
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Le mot de passe et sa confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Rôle")]
        public string Role { get; set; }
        public string? Address { get; set; }
        public string? Telephone { get; set; }
        public IFormFile? PhotoProfile { get; set; }
        public int? ServiceID { get; set; } // Clé étrangère vers Service
        public Service? Service { get; set; }
    }
}
