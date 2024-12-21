using System.ComponentModel.DataAnnotations;

namespace MHR_EF_Code.ViewModels
{
    public class departmentVM
    {
        [Key]
        public Guid DepartmentID { get; set; }
        public string? Name { get; set; }
        public string? location { get; set; }
    }
}
