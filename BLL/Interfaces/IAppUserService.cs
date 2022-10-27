using DAL.Models;

namespace BLL.Interfaces
{
    public interface IAppUserService
    {
        AppUser CreateNewAppUser(string id, bool isHit);
    }
}
