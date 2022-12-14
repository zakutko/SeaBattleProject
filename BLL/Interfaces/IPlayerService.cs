using DAL.Models;

namespace BLL.Interfaces
{
    public interface IPlayerService
    {
        AppUser GetPlayer(string id);
        string GetPlayerId(string username);
    }
}
