using Intuit.Ipp.Data;
using MHR_EF_Code.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MHR_EF_Code.ViewModels;

namespace MHR_EF_Code.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<products> product { get; set; }
        public DbSet<department> department { get; set; }
        public DbSet<Training> training { get; set; }

        public DbSet<EmployeeTraining> EmployeeTraining { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Overtime> Overtime { get; set; }
        public DbSet<Payroll> Payroll { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Bonus> Bonus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Định nghĩa khóa chính composite cho EmployeeTraining
            modelBuilder.Entity<EmployeeTraining>()
                .HasKey(et => new { et.EmployeeId, et.TrainingId });

            modelBuilder.Entity<EmployeeTraining>()
                .Property(et => et.EmployeeId)
                .HasColumnName("EmployeeId");

            modelBuilder.Entity<EmployeeTraining>()
                .Property(et => et.TrainingId)
                .HasColumnName("TrainingId");
            modelBuilder.Entity<EmployeeTraining>()
            .HasOne(et => et.Employees)
            .WithMany(e => e.EmployeeTrainings)
            .HasForeignKey(et => et.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeTraining>()
                .HasOne(et => et.Training)
                .WithMany(t => t.EmployeeTrainings)
                .HasForeignKey(et => et.TrainingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employees>()
             .HasOne(e => e.Attendance)
             .WithOne(a => a.Employee)
             .HasForeignKey<Attendance>(a => a.EmployeeId)
             .OnDelete(DeleteBehavior.Cascade); ;

            modelBuilder.Entity<Employees>()
                .HasOne(e => e.Contact)
                .WithOne(c => c.Employee)
                .HasForeignKey<Contact>(c => c.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade); ;

            modelBuilder.Entity<Employees>()
                .HasOne(e => e.Overtime)
                .WithOne(c => c.Employee)
                .HasForeignKey<Overtime>(c => c.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade); ;

            modelBuilder.Entity<Employees>()
               .HasOne(e => e.Payroll)
               .WithOne(c => c.Employee)
               .HasForeignKey<Payroll>(c => c.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade); ;

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.Employees)
                .WithMany(e => e.LeaveRequests)
                .HasForeignKey(lr => lr.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bonus>()
                .HasOne(b => b.Employees)
                .WithMany(e => e.Bonuses)
                .HasForeignKey(b => b.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);


        }
        public DbSet<MHR_EF_Code.ViewModels.LeaveRequestVM> LeaveRequestVM { get; set; } = default!;


    }
}
