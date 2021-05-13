using RoboticsManagement.Models;
using System.Collections.Generic;

namespace RoboticsManagement.Interfaces.IRepository
{
    public interface IEmployeeTaskRepository
    {
        public EmployeeTask GetTaskById(int taskId);
        public List<EmployeeTask> GetUnDoneTasks();
        public List<EmployeeTask> GetDoneTasks();
        public List<EmployeeTask> SortAscById();
        public void AddEmployeeTask(EmployeeTask empTask);
    }
}
