using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHR_EF_Code.Models.Entities
{
    public class EmployeeTraining
    {
        [Key]
        [Column(Order = 0)]
        public Guid EmployeeId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid TrainingId { get; set; }  // Sửa kiểu dữ liệu thành Guid

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("TrainingId")]
        public virtual Training Training { get; set; }
    }
}
