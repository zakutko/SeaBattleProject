using DAL.Models;

namespace BLL.Interfaces
{
    public interface IGameService
    {
        IEnumerable<Game> GetGames();
        Game GetGame(int id);
        string GetPlayerId(string username);
    }
}
