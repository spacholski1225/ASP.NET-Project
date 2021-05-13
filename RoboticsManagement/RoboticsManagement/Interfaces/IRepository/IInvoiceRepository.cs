using RoboticsManagement.Models;

namespace RoboticsManagement.Interfaces.IRepository
{
    public interface IInvoiceRepository
    {
        public void AddInvoice(InvoiceData invoiceData);
    }
}
