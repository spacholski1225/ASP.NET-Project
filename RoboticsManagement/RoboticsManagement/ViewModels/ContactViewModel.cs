using RoboticsManagement.Models.Enums;

namespace RoboticsManagement.ViewModels
{
    public class ContactViewModel
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public string Topic { get; set; }
        public EContact EContact  { get; set; }
    }
}
