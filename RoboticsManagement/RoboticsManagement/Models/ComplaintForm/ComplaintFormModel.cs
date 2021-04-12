using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Models.ComplaintForm
{
    public class ComplaintFormModel
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }

        public ERobotsCategory ERobotsCategory { get; set; }
    }
}
