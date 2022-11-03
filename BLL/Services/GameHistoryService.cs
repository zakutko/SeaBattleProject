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

        public (string, int)[] GetAllGameHistorySortedByDescOrderOfOccurrenceInTheTable(IEnumerable<GameHistory> gameHistories)
        {
            var firstPlace = string.Empty;
            var secondPlace = string.Empty;
            var thirdPlace = string.Empty;

            var firstPlaceNumberOfWins = 0;
            var secondPlaceNumberOfWins = 0;
            var thirdPlaceNumberOfWins = 0;

            if (gameHistories.Select(x => x.WinnerName).Distinct().Count() == 1) 
            {
                firstPlace = gameHistories.GroupBy(x => x.WinnerName).OrderByDescending(g => g.Count()).ToArray()[0].Key;
                firstPlaceNumberOfWins = gameHistories.Where(x => x.WinnerName == firstPlace).Count();
            }
            else if (gameHistories.Select(x => x.WinnerName).Distinct().Count() == 2)
            {
                firstPlace = gameHistories.GroupBy(x => x.WinnerName).OrderByDescending(g => g.Count()).ToArray()[0].Key;
                secondPlace = gameHistories.GroupBy(x => x.WinnerName).OrderByDescending(g => g.Count()).ToArray()[1].Key;
                firstPlaceNumberOfWins = gameHistories.Where(x => x.WinnerName == firstPlace).Count();
                secondPlaceNumberOfWins = gameHistories.Where(x => x.WinnerName == secondPlace).Count();
            }
            else
            {
                firstPlace = gameHistories.GroupBy(x => x.WinnerName).OrderByDescending(g => g.Count()).ToArray()[0].Key;
                secondPlace = gameHistories.GroupBy(x => x.WinnerName).OrderByDescending(g => g.Count()).ToArray()[1].Key;
                thirdPlace = gameHistories.GroupBy(x => x.WinnerName).OrderByDescending(g => g.Count()).ToArray()[2].Key;

                firstPlaceNumberOfWins = gameHistories.Where(x => x.WinnerName == firstPlace).Count();
                secondPlaceNumberOfWins = gameHistories.Where(x => x.WinnerName == secondPlace).Count();
                thirdPlaceNumberOfWins = gameHistories.Where(x => x.WinnerName == thirdPlace).Count();
            }

            var arrayResult = new (string, int)[]
            {
                (firstPlace, firstPlaceNumberOfWins),
                (secondPlace, secondPlaceNumberOfWins),
                (thirdPlace, thirdPlaceNumberOfWins)
            };

            return arrayResult;
        }
    }
}