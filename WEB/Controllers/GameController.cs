using BLL.Handlers.Fields;
using BLL.Handlers.GameFields;
using BLL.Handlers.Games;
using BLL.Handlers.PlayerGames;
using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WEB.ViewModels;

namespace WEB.Controllers
{
    [AllowAnonymous]
    public class GameController : BaseController
    {
        private readonly IGameService _gameService;
        private readonly IPlayerService _playerService;
        private readonly IGameStateService _gameStateService;
        private readonly IPlayerGameService _playerGameService;
        private readonly IGameFieldService _gameFieldService;
        public GameController(IGameService gameService, IPlayerService playerService, IGameStateService gameStateService, IPlayerGameService playerGameService, IGameFieldService gameFieldService)
        {
            _gameService = gameService;
            _playerService = playerService;
            _gameStateService = gameStateService;
            _playerGameService = playerGameService;
            _gameFieldService = gameFieldService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GameListViewModel>>> GetGames(string token)
        {
            var playerGameList = await Mediator.Send(new List.Query());

            var playerGameListAll = CreateGameListViewModel(playerGameList);
            var playerGameListWithoutCurrUser = new List<GameListViewModel>();

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;

            foreach(var playerGame in playerGameListAll)
            {
                if (playerGame.FirstPlayer != username && playerGame.SecondPlayer != username)
                {
                    playerGameListWithoutCurrUser.Add(playerGame);
                }
            }

            return Ok(playerGameListWithoutCurrUser);
        }
        private List<GameListViewModel> CreateGameListViewModel(List<PlayerGame> playerGameList)
        {
            var gameListViewModel = new List<GameListViewModel>();

            foreach(var playerGame in playerGameList)
            {
                var game = _gameService.GetGame(playerGame.GameId);
                var firstPlayer = _playerService.GetPlayer(playerGame.FirstPlayerId);
                var secondPlayer = _playerService.GetPlayer(playerGame.SecondPlayerId);
                var gameState = _gameStateService.GetGameState(game.GameStateId);

                var numberOfPlayers = _playerGameService.GetNumberOfPlayers(playerGame.GameId);

                gameListViewModel.Add(new GameListViewModel
                {
                    Id = game.Id,
                    FirstPlayer = firstPlayer.UserName,
                    SecondPlayer = secondPlayer?.UserName,
                    GameState = gameState.GameStateName,
                    NumberOfPlayers = numberOfPlayers
                });
            }
            return gameListViewModel;
        }

        [HttpGet("createGame")]
        public async Task<IActionResult> CreateGame(string token)
        {
            var game = new Game { GameStateId = 1 };
            await Mediator.Send(new CreateGame.Command { Game = game });

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;

            var playerId = _gameService.GetPlayerId(username);
            var playerGame = new PlayerGame { GameId = game.Id, FirstPlayerId = playerId };
            await Mediator.Send(new CreatePlayerGame.Command { PlayerGame = playerGame });

            var field = new Field { Size = 10, PlayerId = playerId };
            await Mediator.Send(new CreateField.Command { Field = field });

            var gameField = new GameField { FirstFieldId = field.Id, GameId = game.Id };
            await Mediator.Send(new CreateGameField.Command { GameField = gameField });
            return Ok();
        }

        [HttpGet("joinSecondPlayer")]
        public async Task<IActionResult> JoinSecondPlayer(int gameId, string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;
            var secondPlayerId = _gameService.GetPlayerId(username);
            var firstPlayerId = _playerGameService.GetFirstPlayerId(gameId);
            var playerGameId = _playerGameService.GetPlayerGameId(gameId, firstPlayerId);
            var firstFieldId = _gameFieldService.GetFirstFieldId(gameId);
            var gameFieldId = _gameFieldService.GetGameFieldId(gameId, firstFieldId);

            //update table Game
            var game = new Game { Id = gameId, GameStateId = 2 };
            await Mediator.Send(new UpdateGame.Command { Game = game });

            //update table Field
            var field = new Field { Size = 10, PlayerId = secondPlayerId };
            await Mediator.Send(new CreateField.Command { Field = field });

            //update table PlayerGame
            var playerGame = new PlayerGame { Id = playerGameId, GameId = gameId, FirstPlayerId = firstPlayerId, SecondPlayerId = secondPlayerId };
            await Mediator.Send(new UpdatePlayerGame.Command { PlayerGame = playerGame });

            //update table GameField
            var gameField = new GameField { Id = gameFieldId, FirstFieldId = firstFieldId, SecondFieldId = field.Id, GameId = gameId };
            await Mediator.Send(new UpdateGameField.Command { GameField = gameField });

            return Ok();
        }
    }
}