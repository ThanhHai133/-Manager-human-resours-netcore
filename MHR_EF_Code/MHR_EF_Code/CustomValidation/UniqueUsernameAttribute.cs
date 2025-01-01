using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MHR_EF_Code.CustomValidation
{
    public class UniqueUsernameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Username is required.");
            }

            // Lấy UserManager từ DI container
            var userManager = (UserManager<IdentityUser>)validationContext.GetService(typeof(UserManager<IdentityUser>));
            if (userManager == null)
            {
                throw new System.Exception("UserManager is not configured.");
            }

            var username = value.ToString();

            // Kiểm tra username trong cơ sở dữ liệu
            var user = Task.Run(() => userManager.FindByNameAsync(username)).Result;

            if (user != null)
            {
                return new ValidationResult($"The username '{username}' is already taken.");
            }

            return ValidationResult.Success;
        }
    }
}
