namespace MHR_EF_Code.ViewModels
{
    public class EmployeeDetailVM
    {
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public DateTime HireDate { get; set; }
        public string Identity { get; set; }
        public string Education { get; set; }
        public string Photo { get; set; }
        public Guid DepartmentID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Position { get; set; }
    }

}
