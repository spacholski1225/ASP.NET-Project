using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Models
{
    public class EmployeeTask
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Adress { get; set; }
        public ICollection<TaskForEmployee> TaskForEmployee{ get; set; }
        public bool isDone { get; set; }
    }
}
