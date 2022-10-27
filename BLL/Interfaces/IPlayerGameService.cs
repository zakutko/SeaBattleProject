using DAL.Models;

namespace BLL.Interfaces
{
    public interface IPlayerGameService
    {
        PlayerGame CreatePlayerGame(int gameId, string playerId);
        PlayerGame UpdatePlayerGame(int playerGameId, int gameId, string firstPlayerId, string secondPlayerId);
        int GetNumberOfPlayers(int id);
        string GetFirstPlayerId(int id);
        int GetPlayerGameId(int id, string firstPlayerId);
        string GetSecondPlayerId(string firstPlayerId);
        int GetNumberOfReadyPlayers(string firstPlayerId, string secondPlayerId);
        PlayerGame GetPlayerGame(string firstPlayerId, string secondPlayerId);
        PlayerGame CreateNewPlayerGame(PlayerGame playerGame);
    }
}
