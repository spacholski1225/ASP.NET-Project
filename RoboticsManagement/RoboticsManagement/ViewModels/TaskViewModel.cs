using RoboticsManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.ViewModels
{
    public class TaskViewModel
    {
        public int TaskId { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public ERobotsCategory ERobotsCategory { get; set; }
        public string CreatedDate { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Adress { get; set; }
        public int NIP { get; set; }

        public int Regon { get; set; }
        public string AppUserId { get; set; }
    }
}
