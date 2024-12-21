using System.ComponentModel.DataAnnotations;

namespace MHR_EF_Code.Models.Entities
{
    public class department
    {
        [Key]
        public Guid DepartmentID { get; set; }
        public string? Name { get; set; }
        public string? location { get; set; }
    }
}
