using Microsoft.Extensions.Logging;
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
        private readonly ILogger<FormRepository> _logger;

        public FormRepository(MgmtDbContext context,
            ILogger<FormRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public FormModel CreateForm()
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            var form = _context.complaintFormModels.FirstOrDefault(f => f.Id == id);
            try
            {
                _context.complaintFormModels.Remove(form);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                _logger.LogError("Can't remove form by id or save database", ex);
            }
        }

        public FormModel GetFormById(int id) => _context.complaintFormModels.FirstOrDefault(f => f.Id == id);

        public void ModifyForm(FormModel form)
        {
            try
            {
                _context.complaintFormModels.Update(form);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Can't update form information", ex);
            }
            
        }

        public List<FormModel> SortAscById() => _context.complaintFormModels.OrderBy(x => x.Id).ToList();
    }
}
