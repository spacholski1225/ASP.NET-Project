using RoboticsManagement.Models;
using System;

namespace RoboticsManagement.ViewModels
{
    public class SummaryViewModel
    {
        public string Company { get; set; }
        public string Description { get; set; }
        public ERobotsCategory ERobotsCategory { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Adress { get; set; }
        public bool IsOkay { get; set; }
    }
}
