using System;
using System.ComponentModel.DataAnnotations;

namespace RoboticsManagement.ViewModels
{
    public class CompanyInfoViewModel
    {
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string NIP { get; set; }
       
        public string Regon { get; set; }
        
        public string CompanyName { get; set; }
        
        public string ZipCode { get; set; }
        
        public string City { get; set; }
        
        public string Country { get; set; }
        
        public string Adress { get; set; }
    }
}
