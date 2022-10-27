using BLL.Handlers.AppUsers;
using BLL.Handlers.Cells;
using BLL.Handlers.Fields;
using BLL.Handlers.GameFields;
using BLL.Handlers.Games;
using BLL.Handlers.PlayerGames;
using BLL.Handlers.Positions;
using BLL.Handlers.Ships;
using BLL.Handlers.ShipWrappers;
using BLL.Interfaces;
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
        private readonly IShipWrapperService _shipWrapperService;
        private readonly IAppUserService _appUserService;
        private readonly IShipService _shipService;

        public GameController(
            IGameService gameService, 
            IPlayerService playerService, 
            IPlayerGameService playerGameService, 
            IGameFieldService gameFieldService, 
            IGameStateService gameStateService,
            IDirectionService directionService,
            ICellService cellService,
            IPositionService positionService,
            IFieldService fieldService,
            IShipWrapperService shipWrapperService,
            IAppUserService appUserService,
            IShipService shipService)
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
            _shipWrapperService = shipWrapperService;
            _appUserService = appUserService;
            _shipService = shipService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameListViewModel>>> GetGames(string token)
        {
            var playerGameList = await Mediator.Send(new List.Query());

            var playerGameListAll = new List<GameListViewModel>();

            foreach (var playerGame in playerGameList)
            {
                var game = _gameService.GetGame(playerGame.GameId);
                var firstPlayer = _playerService.GetPlayer(playerGame.FirstPlayerId);
                var secondPlayer = _playerService.GetPlayer(playerGame.SecondPlayerId);
                var gameState = _gameStateService.GetGameState(game.GameStateId);

                var numberOfPlayers = _playerGameService.GetNumberOfPlayers(playerGame.GameId);

                playerGameListAll.Add(new GameListViewModel
                {
                    Id = game.Id,
                    FirstPlayer = firstPlayer.UserName,
                    SecondPlayer = secondPlayer?.UserName,
                    GameState = gameState,
                    NumberOfPlayers = numberOfPlayers
                });
            }
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
        
        [HttpGet("createGame")]
        public async Task<IActionResult> CreateGame(string token)
        {
            var game = _gameService.CreateGame(1);
            await Mediator.Send(new CreateGame.Command { Game = game });

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;

            var playerId = _playerService.GetPlayerId(username);
            await Mediator.Send(new UpdateAppUser.Command { AppUser = _appUserService.CreateNewAppUser(playerId, true) });

            var playerGame = _playerGameService.CreatePlayerGame(game.Id, playerId);
            await Mediator.Send(new CreatePlayerGame.Command { PlayerGame = playerGame });

            var field = _fieldService.CreateField(10, playerId);
            await Mediator.Send(new CreateField.Command { Field = field });

            var gameField = _gameFieldService.CreateGameField(field.Id, game.Id);
            await Mediator.Send(new CreateGameField.Command { GameField = gameField });
            return Ok();
        }

        [HttpGet("joinSecondPlayer")]
        public async Task<IActionResult> JoinSecondPlayer(int gameId, string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;
            var secondPlayerId = _playerService.GetPlayerId(username);
            var firstPlayerId = _playerGameService.GetFirstPlayerId(gameId);
            var playerGameId = _playerGameService.GetPlayerGameId(gameId, firstPlayerId);
            var firstFieldId = _gameFieldService.GetFirstFieldId(gameId);
            var gameFieldId = _gameFieldService.GetGameFieldId(gameId, firstFieldId);

            //update table AppUser
            await Mediator.Send(new UpdateAppUser.Command { AppUser = _appUserService.CreateNewAppUser(secondPlayerId, false) });

            //update table Game
            var game = _gameService.UpdateGame(gameId, 2);
            await Mediator.Send(new UpdateGame.Command { Game = game });

            //update table Field
            var field = _fieldService.CreateField(10, secondPlayerId);
            await Mediator.Send(new CreateField.Command { Field = field });

            //update table PlayerGame
            var playerGame = _playerGameService.UpdatePlayerGame(playerGameId, gameId, firstPlayerId, secondPlayerId);
            await Mediator.Send(new UpdatePlayerGame.Command { PlayerGame = playerGame });

            //update table GameField
            var gameField = _gameFieldService.UpdateGameField(gameFieldId, firstFieldId, field.Id, gameId);
            await Mediator.Send(new UpdateGameField.Command { GameField = gameField });

            return Ok();
        }

        [HttpGet("prepareGame")]
        public async Task<ActionResult<IEnumerable<CellListViewModel>>> GetCells(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;
            var playerId = _playerService.GetPlayerId(username);

            var fieldId = _fieldService.GetFieldId(playerId);

            var shipWrappers = _shipWrapperService.GetAllShipWrappersByFiedlId(fieldId);

            var positions = _positionService.GetAllPoitionsByShipWrapperId(shipWrappers);

            var cellsIds = _cellService.GetAllCellsIdByPositions(positions);

            var cellList = _cellService.GetAllCellsByCellIds(cellsIds);

            var cellListViewModels = new List<CellListViewModel>();

            foreach (var cell in cellList)
            {
                cellListViewModels.Add(new CellListViewModel
                {
                    Id = cell.Id,
                    X = cell.X,
                    Y = cell.Y,
                    CellStateId = cell.CellStateId
                });
            }
            return Ok(cellListViewModels.OrderBy(x => x.Id));
        }

        [HttpPost("prepareGame/createShipOnField")]
        public async Task<IActionResult> CreateShipOnField([FromBody] CreateShipViewModel model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(model.Token);
            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;

            var playerId = _playerService.GetPlayerId(username);
            var fieldId = _fieldService.GetFieldId(playerId);

            var numberOfShipsOnField = _shipWrapperService.GetNumberOfShips(fieldId);

            if (numberOfShipsOnField == 0)
            {
                var defaultCells = _cellService.SetDefaultCells();

                foreach (var cell in defaultCells)
                {
                    await Mediator.Send(new UpdateCell.Command { Cell = cell });
                }

                var defaultShipWrapper = _shipWrapperService.GetDefaultShipWrapper(fieldId);
                await Mediator.Send(new CreateShipWrapper.Command { ShipWrapper = defaultShipWrapper });

                var defaultPositions = _positionService.SetDefaultPositions(defaultShipWrapper.Id, defaultCells);

                foreach (var position in defaultPositions)
                {
                    await Mediator.Send(new CreatePosition.Command { Position = position });
                }
            }

            else if (numberOfShipsOnField == 10)
            {
                return BadRequest("There are already 10 ships on the field!");
            }

            switch (model.ShipSize)
            {
                case 1:
                    if (_shipWrapperService.GetNumberOfShipsWhereSizeOne(fieldId) == 4)
                    {
                        return BadRequest("The maximum number of ships with the size 1 on the field is 4!");
                    }
                    break;
                case 2:
                    if (_shipWrapperService.GetNumberOfShipsWhereSizeTwo(fieldId) == 3)
                    {
                        return BadRequest("The maximum number of ships with the size 2 on the field is 3!");
                    }
                    break;
                case 3:
                    if (_shipWrapperService.GetNumberOfShipsWhereSizeThree(fieldId) == 2)
                    {
                        return BadRequest("The maximum number of ships with the size 3 on the field is 2!");
                    }
                    break;
                case 4:
                    if (_shipWrapperService.GetNumberOfShipsWhereSizeFour(fieldId) == 1)
                    {
                        return BadRequest("The maximum number of ships with the size 4 on the field is 1!");
                    }
                    break;
            }

            var ship = _shipService.CreateShip(model.ShipDirection, 1, model.ShipSize);
            //add ship to Ship table
            await Mediator.Send(new CreateShip.Command { Ship = ship });

            var shipWrapper = _shipWrapperService.CreateShipWrapper(ship.Id, fieldId);
            //add shipWrapper to ShipWrapper table
            await Mediator.Send(new CreateShipWrapper.Command { ShipWrapper = shipWrapper });

            var shipDirectionName = _directionService.GetDirectionName(model.ShipDirection);

            var cellList = _cellService.GetAllCells(shipDirectionName, model.ShipSize, model.X, model.Y, fieldId);

            if (!cellList.Any())
            {
                await Mediator.Send(new DeleteShipWrapper.Command { ShipWrapper = shipWrapper });
                await Mediator.Send(new DeleteShip.Command { Ship = ship });
                return BadRequest("The place is occupied by another ship!");
            }

            foreach (var cell in cellList)
            {
                try
                {
                    var shipWrappers = _shipWrapperService.GetAllShipWrappersByFiedlId(fieldId);
                    //update cells in Cell table
                    var cellId = _cellService.GetCellId(cell.X, cell.Y, shipWrappers);
                    var updateCell = _cellService.UpdateCell(cellId, cell.X, cell.Y, cell.CellStateId);
                    await Mediator.Send(new UpdateCell.Command { Cell = updateCell });

                    //update positions in Position table
                    var positionId = _positionService.GetPositionByCellId(cellId);
                    var updatePosition = _positionService.UpdatePosition(positionId, shipWrapper.Id, cellId);
                    await Mediator.Send(new UpdatePosition.Command { Position = updatePosition });
                }
                catch (Exception ex)
                {
                    await Mediator.Send(new DeleteShipWrapper.Command { ShipWrapper = shipWrapper });
                    await Mediator.Send(new DeleteShip.Command { Ship = ship });
                    return BadRequest(ex.Message);
                }
            }
            return Ok("Ship added successfully!");
        }

        [HttpGet("isPlayerReady")]
        public async Task<IActionResult> IsPlayerReady(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;
            var firstPlayerId = _playerService.GetPlayerId(username);

            var fieldId = _fieldService.GetFieldId(firstPlayerId);
            var shipWrappers = _shipWrapperService.GetAllShipWrappersByFiedlId(fieldId);
            if (shipWrappers.Count() < 10)
            {
                return BadRequest("Number of ships must be 10!");
            }
            var secondPlayerId = _playerGameService.GetSecondPlayerId(firstPlayerId);

            var playerGame = _playerGameService.GetPlayerGame(firstPlayerId, secondPlayerId);
            var newPlayerGame = _playerGameService.CreateNewPlayerGame(playerGame);

            await Mediator.Send(new UpdatePlayerGame.Command { PlayerGame = newPlayerGame });

            return Ok("Player is ready!");
        }

        [HttpGet("isTwoPlayersReady")]
        public async Task<ActionResult<IsReadyViewModel>> IsTwoPlayersReady(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;
            var firstPlayerId = _playerService.GetPlayerId(username);
            var secondPlayerId = _playerGameService.GetSecondPlayerId(firstPlayerId);

            var numberOfReadyPlayers = _playerGameService.GetNumberOfReadyPlayers(firstPlayerId, secondPlayerId);
            var isReadyViewModel = new IsReadyViewModel { NumberOfReadyPlayers = numberOfReadyPlayers };
            return Ok(isReadyViewModel);
        }

        [HttpGet("game/secondPlayerCells")]
        public async Task<ActionResult<IEnumerable<CellListViewModel>>> GetAllCellsForGame(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;
            var firstPlayerId = _playerService.GetPlayerId(username);

            var secondPlayerId = _playerGameService.GetSecondPlayerId(firstPlayerId);
            
            var fieldId = _fieldService.GetFieldId(secondPlayerId);

            var shipWrappers = _shipWrapperService.GetAllShipWrappersByFiedlId(fieldId);

            var positions = _positionService.GetAllPoitionsByShipWrapperId(shipWrappers);

            var cellsIds = _cellService.GetAllCellsIdByPositions(positions);

            var cellList = _cellService.GetAllCellsByCellIds(cellsIds);

            var cellListViewModels = new List<CellListViewModel>();

            foreach (var cell in cellList)
            {
                cellListViewModels.Add(new CellListViewModel
                {
                    Id = cell.Id,
                    X = cell.X,
                    Y = cell.Y,
                    CellStateId = cell.CellStateId
                });
            }

            return Ok(cellListViewModels.OrderBy(x => x.Id));
        }

        [HttpPost("game/fire")]
        public async Task<IActionResult> Fire([FromBody] ShootViewModel model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(model.Token);
            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;
            var firstPlayerId = _playerService.GetPlayerId(username);

            var secondPlayerId = _playerGameService.GetSecondPlayerId(firstPlayerId);
            var fieldId = _fieldService.GetFieldId(secondPlayerId);
            var shipWrappers = _shipWrapperService.GetAllShipWrappersByFiedlId(fieldId);
            var positions = _positionService.GetAllPoitionsByShipWrapperId(shipWrappers);
            var cellsIds = _cellService.GetAllCellsIdByPositions(positions);
            var cellList = _cellService.GetAllCellsByCellIds(cellsIds);
            var cell = _cellService.GetCell(model.X, model.Y, cellList);

            if (cell.CellStateId == 1 || cell.CellStateId == 5)
            {
                var newCell = _cellService.CreateNewCell(cell.Id, cell.X, cell.Y, cell.CellStateId, false);
                await Mediator.Send(new UpdateCell.Command { Cell = newCell });
                var firstAppUser = _appUserService.CreateNewAppUser(firstPlayerId, false);
                await Mediator.Send(new UpdateAppUser.Command { AppUser = firstAppUser });
                var secondAppUser = _appUserService.CreateNewAppUser(secondPlayerId, true);
                await Mediator.Send(new UpdateAppUser.Command { AppUser = secondAppUser });
                return Ok("Missed the fire!");
            }
            else
            {
                var shipWrapperId = _positionService.GetShipWrapperIdByCellId(cell.Id);
                var positionsByShipWrapperId = _positionService.GetAllPoitionsByShipWrapperId(shipWrapperId);
                var cellIds = _positionService.GetAllCellIdsByPositions(positionsByShipWrapperId);
                var cellsByCellIds = _cellService.GetAllCellsByCellIds(cellIds);
                var isDestroyed = _cellService.CheckIfTheShipIsDestroyed(cellsByCellIds);
                var newCell = _cellService.CreateNewCell(cell.Id, cell.X, cell.Y, cell.CellStateId, false);
                await Mediator.Send(new UpdateCell.Command { Cell = newCell });

                if (isDestroyed)
                {
                    foreach (var cellByCellId in cellsByCellIds)
                    {
                        var newCellBtCellId = _cellService.CreateNewCell(cellByCellId.Id, cellByCellId.X, cellByCellId.Y, cellByCellId.CellStateId, isDestroyed);
                        await Mediator.Send(new UpdateCell.Command { Cell = newCellBtCellId });
                    }
                    return Ok("The ship is destroyed!");
                }
                return Ok("The ship is hit!");
            }
        }

        [HttpGet("game/priority")]
        public async Task<ActionResult<HitViewModel>> GetPriopity(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;
            var playerId = _playerService.GetPlayerId(username);

            var player = _playerService.GetPlayer(playerId);

            return new HitViewModel { IsHit = player.IsHit };
        }
    }
}