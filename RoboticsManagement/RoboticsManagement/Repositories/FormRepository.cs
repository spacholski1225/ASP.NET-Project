using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models.ComplaintForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Repositories
{
    public abstract class FormRepository : IFormRepository
    {
        public FormModel CreateForm()
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public FormModel GetFormById(int id)
        {
            throw new NotImplementedException();
        }

        public FormModel ModifyForm(FormModel form)
        {
            throw new NotImplementedException();
        }
    }
}
