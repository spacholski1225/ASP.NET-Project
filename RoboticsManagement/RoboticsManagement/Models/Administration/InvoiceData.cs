using System.ComponentModel.DataAnnotations;

namespace RoboticsManagement.Models
{
    public class InvoiceData
    {
        [Key]
        public int Id { get; set; }
        public string InvoiceXML { get; set; }
    }
}
