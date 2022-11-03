using DAL.Data;
using DAL.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class AppUserRepository : Repository<AppUser>, IAppUserRepository, IRepository<AppUser>
    {
        public AppUserRepository(DataContext context) : base(context)
        {
        }

        public AppUser GetByUsername(string username)
        {
            return _context.AppUsers.Where(u => u.UserName == username).FirstOrDefault();
        }
    }
}
