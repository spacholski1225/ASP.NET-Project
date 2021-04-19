using Microsoft.AspNetCore.Identity;

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
    }
}
