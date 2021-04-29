using Microsoft.AspNetCore.Identity;
using RoboticsManagement.Models.ComplaintForm;
using System.Collections.Generic;

namespace RoboticsManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
       
        public int NIP { get; set; }
        
        public int Regon { get; set; }
        
        public string CompanyName { get; set; }
       
        public string ZipCode { get; set; }
        
        public string City { get; set; }
        
        public string Country { get; set; }
        
        public string Adress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<TaskForEmployee> TaskForEmployee { get; set; }
        public ICollection<FormModel> FormModels{ get; set; }
    }
}
