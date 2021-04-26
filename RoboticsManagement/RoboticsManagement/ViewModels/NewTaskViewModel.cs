﻿namespace RoboticsManagement.ViewModels
{
    public class NewTaskViewModel
    {
        public int TaskId { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Adress { get; set; }
        public EmployeeViewModel Employee { get; set; }
        
    }
}
