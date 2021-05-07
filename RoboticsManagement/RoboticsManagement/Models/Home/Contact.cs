using RoboticsManagement.Models.Enums;

namespace RoboticsManagement.Models.Home
{
    public class Contact
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string Topic { get; set; }
        public EContact EContact { get; set; }
    }
}
