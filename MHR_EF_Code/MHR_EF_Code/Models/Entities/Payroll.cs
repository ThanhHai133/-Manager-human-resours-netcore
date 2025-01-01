using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MHR_EF_Code.Models.Entities
{
    public class Payroll
    {
        [Key]
        public Guid PayrollId { get; set; }  // Khóa chính

        [ForeignKey("Employee")]
        public Guid? EmployeeId { get; set; }  // Khóa ngoại trỏ tới bảng Employees

        [DataType(DataType.Currency)]
        public decimal? TotalSalary { get; set; }  // Tổng lương

        public DateTime? PayDate { get; set; }  // Ngày trả lương

        public string? Details { get; set; }  // Chi tiết lương

        // Điều hướng tới bảng Employees
        public virtual Employees? Employee { get; set; }
    }
}
