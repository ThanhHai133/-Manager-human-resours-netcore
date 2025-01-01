using Intuit.Ipp.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHR_EF_Code.Models.Entities
{
    public class Contact
    {
        public Guid ContactId { get; set; } // Sẽ tự động sinh ra khi thêm mới Contact
        public string ContactType { get; set; }
        public string Description { get; set; }
        public decimal BaseSalary { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        [ForeignKey("Employee")]
        public Guid EmployeeId { get; set; }

        // Mối quan hệ 1:1 với Employee
        public virtual Employees? Employee { get; set; }
    }
}
