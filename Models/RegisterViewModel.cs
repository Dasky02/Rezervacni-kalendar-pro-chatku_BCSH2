using System.ComponentModel.DataAnnotations;

namespace ChatkaReservation.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email je povinný.")]
        [EmailAddress(ErrorMessage = "Neplatný formát emailu.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Heslo je povinné.")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potvrďte heslo")]
        [Compare("Password", ErrorMessage = "Hesla se neshodují.")]
        public required string ConfirmPassword { get; set; }
    }
}