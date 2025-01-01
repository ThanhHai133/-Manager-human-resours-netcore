using System.ComponentModel.DataAnnotations;

namespace MHR_EF_Code.ViewModels
{
    public class LeaveRequestVM
    {
        [Key]
        public Guid LeaveRequestId { get; set; }
        [Required]
        public Guid EmployeeId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        [StringLength(500)]
        public string Reason { get; set; }
        public bool IsApproved { get; set; } = false;
    }
}
