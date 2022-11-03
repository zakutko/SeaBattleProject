using DAL.Models;

namespace BLL.Interfaces
{
    public interface IGameHistoryService
    {
        GameHistory CreateGameHistory(int gameId, string firstPlayerName, string secondPlayerName, string gameStateName, string winnerName);
        bool CheckIfExistGameHistoryByGameId(int gameId);
        (string, int)[] GetAllGameHistorySortedByDescOrderOfOccurrenceInTheTable(IEnumerable<GameHistory> gameHistories);
    }
}
