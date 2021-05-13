using RoboticsManagement.Models.ComplaintForm;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoboticsManagement.Interfaces.IRepository
{
    public interface IFormRepository
    {
        public FormModel GetFormById(int id);
        public void DeleteById(int id);
        public void ModifyForm(FormModel form);
        public FormModel CreateForm();
        public List<FormModel> SortAscById();
        public Task<List<FormModel>> GetAllFormsByUser(string name);
        public void AddTask(FormModel model);
        public FormModel GetFormByUserId(string userId);
    }
}
