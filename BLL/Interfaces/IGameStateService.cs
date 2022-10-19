using DAL.Models;

namespace BLL.Interfaces
{
    public interface IGameStateService
    {
        GameState GetGameState(int id);
    }
}
