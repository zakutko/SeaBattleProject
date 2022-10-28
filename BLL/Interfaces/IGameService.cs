using DAL.Models;

namespace BLL.Interfaces
{
    public interface IGameService
    {
        Game CreateGame(int gameStateId);
        Game UpdateGame(int gameId, int gameStateId);
        IEnumerable<Game> GetGames();
        Game GetGame(int id);
        Game GetNewGame(int id);
    }
}
