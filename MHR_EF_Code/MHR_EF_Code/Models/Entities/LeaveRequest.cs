using Intuit.Ipp.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MHR_EF_Code.Models.Entities
{
    public class LeaveRequest
    {
        [Key] 
        public Guid LeaveRequestId { get; set; }
        [Required] 
        public Guid EmployeeId { get; set; }
        [Required] 
        public DateTime StartDate { get; set; }
        [Required] 
        public DateTime EndDate { get; set; }
        [Required][StringLength(500)] 
        public string Reason { get; set; }
        public bool IsApproved { get; set; } = false; 
        [ForeignKey("EmployeeId")] 
        public virtual Employees Employees { get; set; }
    }
}
