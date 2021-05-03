using RoboticsManagement.Data;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Repositories
{
    public class EmployeeTaskRepository : IEmployeeTaskRepository
    {
        private readonly MgmtDbContext _context;

        public EmployeeTaskRepository(MgmtDbContext context)
        {
            _context = context;
        }
        public EmployeeTask GetTaskById(int taskId)
            => _context.EmployeeTasks.FirstOrDefault(x => x.Id == taskId);

        public List<EmployeeTask> GetDoneTasks() => _context.EmployeeTasks.Where(x => x.isDone == true).ToList();

        public List<EmployeeTask> GetUnDoneTasks() => _context.EmployeeTasks.Where(x => x.isDone == false).ToList();

        public List<EmployeeTask> SortAscById()
        {
            var tasks =  _context.EmployeeTasks.Where(x=> x.Description !=null && x.City !=null && x.Country != null && x.Adress != null).OrderBy(x => x.Id).ToList();
            return tasks;
        }
    }
}
