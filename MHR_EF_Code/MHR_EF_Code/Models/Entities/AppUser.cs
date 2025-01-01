using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHR_EF_Code.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Address { get; set; }

        [ForeignKey("Employee")]
        public Guid? EmployeeId { get; set; }
        public virtual Employees? Employee { get; set; }

    }
}
