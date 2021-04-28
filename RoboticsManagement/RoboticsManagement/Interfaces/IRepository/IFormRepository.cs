using RoboticsManagement.Models.ComplaintForm;

namespace RoboticsManagement.Interfaces.IRepository
{
    public interface IFormRepository
    {
        public FormModel GetFormById(int id);
        public void DeleteById(int id);
        public void ModifyForm(FormModel form);
        public FormModel CreateForm();

    }
}
