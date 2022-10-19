using BLL.Handlers.Cells;
using BLL.Handlers.Fields;
using BLL.Handlers.GameFields;
using BLL.Handlers.Games;
using BLL.Handlers.PlayerGames;
using BLL.Handlers.Positions;
using BLL.Handlers.Ships;
using BLL.Handlers.ShipWrappers;
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
        private readonly IPlayerGameService _playerGameService;
        private readonly IGameFieldService _gameFieldService;
        private readonly IGameStateService _gameStateService;
        private readonly IDirectionService _directionService;
        private readonly ICellService _cellService;
        private readonly IPositionService _positionService;
        private readonly IFieldService _fieldService;

        public GameController(
            IGameService gameService, 
            IPlayerService playerService, 
            IPlayerGameService playerGameService, 
            IGameFieldService gameFieldService, 
            IGameStateService gameStateService,
            IDirectionService directionService,
            ICellService cellService,
            IPositionService positionService,
            IFieldService fieldService)
        {
            _gameService = gameService;
            _playerService = playerService;
            _playerGameService = playerGameService;
            _gameFieldService = gameFieldService;
            _gameStateService = gameStateService;
            _directionService = directionService;
            _cellService = cellService;
            _positionService = positionService;
            _fieldService = fieldService;
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
                    GameState = gameState,
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

        [HttpPost("prepareGame")]
        public async Task<IActionResult> CreateShipOnField(int shipSize, int shipDirection, int x, int y, string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;

            var playerId = _gameService.GetPlayerId(username);
            var fieldId = _fieldService.GetFieldId(playerId);

            var ship = new Ship { DirectionId = shipDirection, ShipStateId = 1, ShipSizeId = shipSize };

            //add ship to Ship table
            await Mediator.Send(new CreateShip.Command { Ship = ship });

            var shipWrapper = new ShipWrapper { ShipId = ship.Id, FieldId = fieldId };

            //add shipWrapper to ShipWrapper table
            await Mediator.Send(new CreateShipWrapper.Command { ShipWrapper = shipWrapper });

            var shipDirectionName = _directionService.GetDirectionName(shipDirection);
            var cellList = _cellService.getAllCells(shipDirectionName, shipSize, x, y);

            //add cells to Cell table
            foreach (var cell in cellList)
            {
                await Mediator.Send(new CreateCell.Command { Cell = cell });
            }

            var positionList = _positionService.GetAllPositions(shipWrapper.Id, cellList);

            //add positions to Position table
            foreach (var position in positionList)
            {
                await Mediator.Send(new CreatePosition.Command { Position = position });
            }
            return Ok();
        }
    }
}