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
        public DbSet<FormModel> complaintFormModels { get; set; } //note after change table name migration have build error
        public DbSet<EmployeeTask> EmployeeTasks { get; set; } //todo change it into relation one to many where employee can have many tasks
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
