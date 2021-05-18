using RoboticsManagement.Data;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models.Home;

namespace RoboticsManagement.Repositories
{
    public class ContactRepository : IConctactRepository
    {
        private readonly MgmtDbContext context;

        public ContactRepository(MgmtDbContext context)
        {
            this.context = context;
        }
        public void SaveToDatabase(Contact contactModel)
        {
            context.ContactMessages.Add(contactModel);
            context.SaveChanges();
        }
    }
}
