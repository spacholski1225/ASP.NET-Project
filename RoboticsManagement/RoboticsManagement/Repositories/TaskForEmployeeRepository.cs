using RoboticsManagement.Data;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace RoboticsManagement.Repositories
{
    public class TaskForEmployeeRepository : ITaskForEmployeeRepository
    {
        private readonly MgmtDbContext _context;

        public TaskForEmployeeRepository(MgmtDbContext context)
        {
            _context = context;
        }

        public void AddNewTaskForEmployee(TaskForEmployee taskForEmployee)
        {
            _context.TaskForEmployee.Add(taskForEmployee);
            _context.SaveChanges();
        }

        public TaskForEmployee GetTaskById(int id)
        {
            return _context.TaskForEmployee.FirstOrDefault(x => x.TaskId == id);
        }

        public List<TaskForEmployee> GetTaskIdByEmployeeId(string employeeId) 
            => _context.TaskForEmployee.Where(x => x.EmployeeId == employeeId).ToList();

        public List<EmployeeTask> GetTasksForEmployee(string employeeId)
        {
            var listOfTasks = new List<EmployeeTask>();
            var tasks = GetTaskIdByEmployeeId(employeeId);
            tasks.ForEach(t =>
            {
                var task = _context.EmployeeTasks.FirstOrDefault(x => (x.Id == t.TaskId && x.isDone == false));
                if (task != null)
                    listOfTasks.Add(task);
            });
            return listOfTasks;
        }
    }
}
