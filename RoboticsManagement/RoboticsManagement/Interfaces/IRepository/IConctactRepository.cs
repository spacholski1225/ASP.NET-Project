using RoboticsManagement.Models.Home;

namespace RoboticsManagement.Interfaces.IRepository
{
    public interface IConctactRepository
    {
        public void SaveToDatabase(Contact contactModel);
    }
}
