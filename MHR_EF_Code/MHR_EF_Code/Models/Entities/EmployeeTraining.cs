using Intuit.Ipp.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHR_EF_Code.Models.Entities
{
    public class EmployeeTraining
    {
        [Key, Column(Order = 0)]
        public Guid EmployeeId { get; set; }

        [Key, Column(Order = 1)]
        public Guid TrainingId { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employees Employees { get; set; }

        [ForeignKey("TrainingId")]
        public virtual Training Training { get; set; }
    }
}
