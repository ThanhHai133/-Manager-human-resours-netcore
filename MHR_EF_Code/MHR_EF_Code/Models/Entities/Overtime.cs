using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MHR_EF_Code.Models.Entities
{
    public class Overtime
    {
        [Key]
        public Guid OvertimeId { get; set; }  // Khóa chính (ID)

        [ForeignKey("Employee")]
        public Guid EmployeeId { get; set; }  // Khóa ngoại trỏ tới bảng Employee

        [Required]
        public DateTime OvertimeDate { get; set; }  // Ngày làm thêm giờ

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Hours must be a positive number.")]
        public decimal Hours { get; set; }  // Số giờ làm thêm

        // Điều hướng tới bảng Employees (quan hệ 1:N)
        public virtual Employees? Employee { get; set; }
    }
}
