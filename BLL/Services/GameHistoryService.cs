using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class GameHistoryService : IGameHistoryService
    {
        private readonly IRepository<GameHistory> _repository;
        public GameHistoryService(IRepository<GameHistory> repository)
        {
            _repository = repository;
        }

        public bool CheckIfExistGameHistoryByGameId(int gameId)
        {
            var gameHistory = _repository.GetAll().Result.Where(x => x.GameId == gameId);
            if (gameHistory.Any())
            {
                return true;
            }
            return false;
        }

        public GameHistory CreateGameHistory(int gameId, string firstPlayerName, string secondPlayerName, string gameStateName, string winnerName)
        {
            return new GameHistory
            {
                GameId = gameId,
                FirstPlayerName = firstPlayerName,
                SecondPlayerName = secondPlayerName,
                GameStateName = gameStateName,
                WinnerName = winnerName
            };
        }

    }
}