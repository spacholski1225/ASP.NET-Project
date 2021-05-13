using RoboticsManagement.Data;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;

namespace RoboticsManagement.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly MgmtDbContext _context;

        public InvoiceRepository(MgmtDbContext context)
        {
            _context = context;
        }
        public void AddInvoice(InvoiceData invoiceData)
        {
            _context.InvoiceData.Add(invoiceData);
            _context.SaveChanges();
        }
    }
}
