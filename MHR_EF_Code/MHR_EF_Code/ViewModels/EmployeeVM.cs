using MHR_EF_Code.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MHR_EF_Code.ViewModels
{
    public class EmployeeVM
    {
        [Required(ErrorMessage = "Employee Code is required")]
        public string? EmployeeCode { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Hire Date is required")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        public string? Identity { get; set; }

        public string? Education { get; set; }

        public string? Photo { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public Guid DepartmentID { get; set; }
    }
}
