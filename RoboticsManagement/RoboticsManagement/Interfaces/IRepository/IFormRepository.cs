using RoboticsManagement.Models.ComplaintForm;
using System.Collections.Generic;

namespace RoboticsManagement.Interfaces.IRepository
{
    public interface IFormRepository
    {
        public FormModel GetFormById(int id);
        public void DeleteById(int id);
        public void ModifyForm(FormModel form);
        public FormModel CreateForm();
        public List<FormModel> SortAscById();

    }
}
