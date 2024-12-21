using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace MHR_EF_Code.Models.Entities
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EmployeeId { get; set; }

        //[ForeignKey("User")]
        //public string UserId { get; set; }
        [ForeignKey("Department")]
        public Guid DepartmentID { get; set; }
        [Required]
        public string? EmployeeCode { get; set; }
        [Required]
        [StringLength(100)]
        public string? FullName { get; set; }
        public DateTime HireDate { get; set; }
        public string? Identity { get; set; }

        public string? Education { get; set; }

        public string? Photo { get; set; }

        public virtual department? Department { get; set; }

    }
}
