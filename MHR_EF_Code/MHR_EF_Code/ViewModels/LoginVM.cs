using System.ComponentModel.DataAnnotations;

namespace MHR_EF_Code.ViewModels
{
    public class LoginVM
    {
        public string? username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")
            ]
        public required string Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
