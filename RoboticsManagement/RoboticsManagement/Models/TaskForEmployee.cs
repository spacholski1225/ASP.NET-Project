using System.ComponentModel.DataAnnotations;

namespace RoboticsManagement.Models
{
    public class TaskForEmployee
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public EmployeeTask EmployeeTask { get; set; }
        public string EmployeeId { get; set; }
        public ApplicationUser Employee { get; set; }

    }
}
