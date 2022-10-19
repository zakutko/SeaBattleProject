using DAL.Models;

namespace DAL.Interfaces
{
    public interface IAppUserRepository : IRepository<AppUser>
    {
        AppUser GetByUsername(string username);
    }
}
