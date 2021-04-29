using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoboticsManagement.Models;
using RoboticsManagement.Models.ComplaintForm;

namespace RoboticsManagement.Data
{
    public class MgmtDbContext : IdentityDbContext<ApplicationUser>
    {
        public MgmtDbContext(DbContextOptions<MgmtDbContext> options) : base(options)
        {

        }
        public DbSet<FormModel> complaintFormModels { get; set; }
        public DbSet<EmployeeTask> EmployeeTasks { get; set; }
        public DbSet<TaskForEmployee> TaskForEmployee{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TaskForEmployee>().HasKey(x => new { x.TaskId, x.EmployeeId });
            modelBuilder.Entity<TaskForEmployee>().HasOne(x => x.EmployeeTask).WithMany(x => x.TaskForEmployee).HasForeignKey(x => x.TaskId);
            modelBuilder.Entity<TaskForEmployee>().HasOne(x => x.Employee).WithMany(x => x.TaskForEmployee).HasForeignKey(x => x.EmployeeId);

            modelBuilder.Entity<ApplicationUser>().HasMany(f => f.FormModels).WithOne(a => a.ApplicationUser);
        }
    }
}
