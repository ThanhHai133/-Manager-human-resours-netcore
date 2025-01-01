using System.ComponentModel.DataAnnotations;

namespace MHR_EF_Code.ViewModels
{
    public class BonusVM
    {
        [Key]
        public Guid BonusId { get; set; }
        [Required]
        public string BonusName { get; set; }
        [Required]
        public string Description { get; set; }
        public decimal Amount { get; set; }
        [Required]
        public Guid EmployeeId { get; set; }
    }
}
