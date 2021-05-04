using RoboticsManagement.Models;

namespace RoboticsManagement.ViewModels
{
    public class SummaryViewModel
    {
        public string Id { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public ERobotsCategory ERobotsCategory { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Adress { get; set; }
        public bool IsOkay { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
