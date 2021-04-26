using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.ViewModels
{
    public class EmployeeViewModel
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public NewTaskViewModel NewTaskViewModel { get; set; }
    }
}
