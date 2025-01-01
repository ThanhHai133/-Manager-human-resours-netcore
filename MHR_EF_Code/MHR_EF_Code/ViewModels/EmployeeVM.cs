using MHR_EF_Code.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MHR_EF_Code.CustomValidation;


namespace MHR_EF_Code.ViewModels
{
    public class EmployeeVM
    {
        public Guid? EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee Code is required.")]
        [StringLength(10, ErrorMessage = "Employee Code cannot exceed 10 characters.")]
        public string? EmployeeCode { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(50, ErrorMessage = "Full Name cannot exceed 50 characters.")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Hire Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Hire Date must be a valid date.")]
        public DateTime HireDate { get; set; }

        [StringLength(12, ErrorMessage = "Identity must be a maximum of 12 characters.")]
        public string? Identity { get; set; }

        [StringLength(100, ErrorMessage = "Education cannot exceed 100 characters.")]
        public string? Education { get; set; }

        //[Url(ErrorMessage = "Photo must be a valid URL.")]
        public string? Photo { get; set; }
        public IFormFile PhotoFile { get; set; }
        public string? Position { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, ErrorMessage = "Username cannot exceed 20 characters.")]
        public required string username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public Guid DepartmentID { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string? CurrentPassword { get; set; }

        // Contact
        [Required(ErrorMessage = "Contact Type is required.")]
        [StringLength(20, ErrorMessage = "Contact Type cannot exceed 20 characters.")]
        public string ContactType { get; set; }

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Base Salary is required.")]
        public decimal BaseSalary { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Start Date must be a valid date.")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "End Date must be a valid date.")]
        public DateTime? EndDate { get; set; }

        // Overtime
        [Required(ErrorMessage = "Hours are required.")]
        public decimal Hours { get; set; }

        [Required(ErrorMessage = "Overtime Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Overtime Date must be a valid date.")]
        public DateTime OvertimeDate { get; set; }
        public int TotalDaysWorked { get; set; }
        public decimal? TotalSalary { get; set; }
    }
}
