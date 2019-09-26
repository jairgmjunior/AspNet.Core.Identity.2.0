using System.ComponentModel.DataAnnotations;

namespace Identity.Core.Models.AccountViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} deve ter 100 caracteres e no minimo {1} caractere", MinimumLength = 8 )]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Senhas não conferem!")]
        public string ConfirmPassword { get; set; }
    }
}
