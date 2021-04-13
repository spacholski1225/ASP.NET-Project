using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Models
{
    public class ReservedTime
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
    }
}
