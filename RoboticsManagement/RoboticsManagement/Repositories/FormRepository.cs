using RoboticsManagement.Data;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models.ComplaintForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Repositories
{
    public class FormRepository : IFormRepository
    {
        private readonly MgmtDbContext _context;
        public FormRepository(MgmtDbContext context)
        {
            _context = context;
        }
        public FormModel CreateForm()
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            var form = _context.complaintFormModels.FirstOrDefault(f => f.Id == id);
            _context.complaintFormModels.Remove(form);
            _context.SaveChanges();
        }

        public FormModel GetFormById(int id) => _context.complaintFormModels.FirstOrDefault(f => f.Id == id);

        public void ModifyForm(FormModel form)
        {
            _context.complaintFormModels.Update(form);
            _context.SaveChanges();
        }

        public List<FormModel> SortAscById() => _context.complaintFormModels.OrderBy(x => x.Id).ToList();
    }
}
