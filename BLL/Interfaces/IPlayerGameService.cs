using DAL.Models;

namespace BLL.Interfaces
{
    public interface IPlayerGameService
    {
        int GetNumberOfPlayers(int id);
        string GetFirstPlayerId(int id);
        int GetPlayerGameId(int id, string firstPlayerId);
        string GetSecondPlayerId(string firstPlayerId);
        int GetNumberOfReadyPlayers(string firstPlayerId, string secondPlayerId);
        PlayerGame GetPlayerGame(string firstPlayerId, string secondPlayerId);
        PlayerGame CreateNewPlayerGame(PlayerGame playerGame);
    }
}
