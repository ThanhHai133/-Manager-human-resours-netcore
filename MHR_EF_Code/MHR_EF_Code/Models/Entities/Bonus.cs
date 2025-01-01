using Intuit.Ipp.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MHR_EF_Code.Models.Entities
{
    public class Bonus
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
        [ForeignKey("EmployeeId")] 
        public virtual Employees Employees { get; set; }
    }
}
