using BLL.Handlers.GameHistories;
using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB.ViewModels;

namespace WEB.Controllers
{
    [AllowAnonymous]
    public class GameHistoryController : BaseController
    {
        private readonly IGameHistoryService _gameHistoryService;

        public GameHistoryController(IGameHistoryService gameHistoryService)
        {
            _gameHistoryService = gameHistoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameHistoryViewModel>>> GetGameHistories()
        {
            var gameHistories = await Mediator.Send(new ListGameHistories.Query());

            var gameHistoryViewModels = new List<GameHistoryViewModel>();

            foreach (var gameHistory in gameHistories)
            {
                gameHistoryViewModels.Add(new GameHistoryViewModel
                {
                    Id = gameHistory.Id,
                    GameId = gameHistory.GameId,
                    FirstPlayerName = gameHistory.FirstPlayerName,
                    SecondPlayerName = gameHistory.SecondPlayerName,
                    GameStateName = gameHistory.GameStateName,
                    WinnerName = gameHistory.WinnerName
                });
            }

            return Ok(gameHistoryViewModels);
        }

        [HttpGet("topPlayers")]
        public async Task<ActionResult<TopPlayersViewModel>> GetTopPlayers()
        {
            var gameHistories = await Mediator.Send(new ListGameHistories.Query());

            var result = _gameHistoryService.GetAllGameHistorySortedByDescOrderOfOccurrenceInTheTable(gameHistories);

            var topPlayers = new TopPlayersViewModel
            {
                FirstPlacePlayer = result[0].Item1,
                SecondPlacePlayer = result[1].Item1,
                ThirdPlacePlayer = result[2].Item1,
                FirstPlaceNumberOfWins = result[0].Item2,
                SecondPlaceNumberOfWins = result[1].Item2,
                ThirdPlaceNumberOfWins = result[2].Item2
            };

            return Ok(topPlayers);
        }
    }
}
