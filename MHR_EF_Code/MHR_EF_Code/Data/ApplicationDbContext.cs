using MHR_EF_Code.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MHR_EF_Code.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        { 
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<products> product{ get; set; }
        public DbSet<department> department{ get; set; }
        public DbSet<Training> training { get; set; }
        public DbSet<EmployeeTraining> EmployeeTraining { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Định nghĩa khóa chính composite cho EmployeeTraining
            modelBuilder.Entity<EmployeeTraining>()
                .HasKey(et => new { et.EmployeeId, et.TrainingId });

            base.OnModelCreating(modelBuilder);
        }


    }
}
