using DAL.Models;

namespace BLL.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
