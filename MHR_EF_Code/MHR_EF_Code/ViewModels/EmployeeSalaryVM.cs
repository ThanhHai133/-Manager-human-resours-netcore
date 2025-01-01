using Intuit.Ipp.Data;
using MHR_EF_Code.Models.Entities;

namespace MHR_EF_Code.ViewModels
{
    public class EmployeeSalaryVM
    {
        public Employees Employees { get; set; }
        public Contact Contact { get; set; }
        public Attendance Attendance { get; set; }
        public Overtime Overtime { get; set; }
        public Payroll Payroll { get; set; }
        public Decimal TotalSalary { get; set; }
        //public int TotalDaysWorked { get; set; }
        //public Decimal Hours {  get; set; }

    }
}
