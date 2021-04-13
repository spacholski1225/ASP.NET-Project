using Microsoft.EntityFrameworkCore;
using RoboticsManagement.Models;
using RoboticsManagement.Models.ComplaintForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Data
{
    public class MgmtDbContext : DbContext
    {
        public MgmtDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<ComplaintFormModel> complaintFormModels { get; set; }
        public DbSet<ReservedTime> ReservedTimes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
