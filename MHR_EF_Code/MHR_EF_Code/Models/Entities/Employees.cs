using Intuit.Ipp.Data;
using MHR_EF_Code.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHR_EF_Code.Models.Entities
{
    public class Employees
    {
        [Key]
        public Guid EmployeeId { get; set; }

        [ForeignKey("department")]
        public Guid DepartmentID { get; set; }

        [ForeignKey("AppUser")]
        public string? UserId1 { get; set; }  // Thêm thuộc tính UserId

        [ForeignKey("Attendance")]
        public Guid? AttendanceId { get; set; }// Khóa ngoại trỏ đến bảng Attendance


        [ForeignKey("Contact")]
        public Guid? ContactId { get; set; }// Khóa ngoại trỏ đến bảng Attendance


        [ForeignKey("Overtime")]
        public Guid? OvertimeId { get; set; }

        [ForeignKey("Payroll")]
        public Guid? PayrollId { get; set; }

        [Required]
        public string? EmployeeCode { get; set; }

        [Required]
        [StringLength(100)]
        public string? FullName { get; set; }
        public string? Position { get; set; }

        public DateTime HireDate { get; set; }

        public string? IdentityNumber { get; set; }

        public string? Education { get; set; }

        public string? Photo { get; set; }
        public virtual department? Department { get; set; }

        public virtual AppUser? User { get; set; }
        // Quan hệ 1:1 với Attendance
        public virtual Attendance? Attendance { get; set; }
        public virtual Contact? Contact { get; set; }
        public virtual Overtime? Overtime { get; set; }
        public virtual Payroll? Payroll { get; set; }
        public virtual ICollection<EmployeeTraining> EmployeeTrainings { get; set; }
        public virtual ICollection<LeaveRequest> LeaveRequests {get; set;}
        public virtual ICollection<Bonus> Bonuses { get; set; }

        public static implicit operator Employee(Employees v)
        {
            throw new NotImplementedException();
        }
    }
}
