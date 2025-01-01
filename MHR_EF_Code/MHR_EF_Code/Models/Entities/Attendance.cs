using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Intuit.Ipp.Data;

namespace MHR_EF_Code.Models.Entities
{
    public class Attendance
    {
        [Key]
        public Guid AttendanceId { get; set; } 

        [Required]
        public int TotalDaysWorked { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        // Quan hệ 1:1 với Employee
        public Guid EmployeeId { get; set; }
        public virtual Employees? Employee { get; set; }
    }
}
