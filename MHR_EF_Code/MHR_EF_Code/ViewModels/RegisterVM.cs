using System.ComponentModel.DataAnnotations;

namespace MHR_EF_Code.ViewModels
{
    public class RegisterVM
    {
        public required string  username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }
        [DataType(DataType.Password)][Display(Name = "Confirm password")][Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}
