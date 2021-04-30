using System;

namespace RoboticsManagement.ViewModels
{
    public class SentFormViewModel
    {
        public int FormId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public string Robot { get; set; }

    }
}
