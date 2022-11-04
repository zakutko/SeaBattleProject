using BLL.Handlers.AppUsers;
using BLL.Handlers.Cells;
using BLL.Handlers.Fields;
using BLL.Handlers.GameFields;
using BLL.Handlers.GameHistories;
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
        private readonly IGameHistoryService _gameHistoryService;

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
            IShipService shipService,
            IGameHistoryService gameHistoryService)
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
            _gameHistoryService = gameHistoryService;
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

        [HttpGet("isGameOwner")]
        public async Task<ActionResult<IsGameOwnerViewModel>> IsGameOwner(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;
            var playerId = _playerService.GetPlayerId(username);

            var isGameOwner = _playerGameService.IsGameOwner(playerId);
            var isSecondPlayerConnected = _playerGameService.IsSecondPlayerConnected(playerId);
            return Ok(new IsGameOwnerViewModel { IsGameOwner = isGameOwner, IsSecondPlayerConnected = isSecondPlayerConnected });
        }

        [HttpGet("deleteGame")]
        public async Task<IActionResult> DeleteGame(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;
            var firstPlayerId = _playerService.GetPlayerId(username);

            var gameId = _playerGameService.GetPlayerGame(firstPlayerId, null).GameId;

            var fieldId = _fieldService.GetFieldId(firstPlayerId);
            var shipIds = _shipWrapperService.GetAllShipIdsByFieldId(fieldId);
            var ships = _shipService.GetAllShips(shipIds);
            var shipWrappers = _shipWrapperService.GetAllShipWrappersByFiedlId(fieldId);
            var positions = _positionService.GetAllPoitionsByShipWrapperId(shipWrappers);
            var cellsIds = _cellService.GetAllCellsIdByPositions(positions);
            var cellList = _cellService.GetAllCellsByCellIds(cellsIds);

            if (cellList.Any())
            {
                //delete all cells
                foreach (var cell in cellList)
                {
                    await Mediator.Send(new DeleteCell.Command { Cell = cell });
                }

                //delete all ships
                foreach (var ship in ships)
                {
                    await Mediator.Send(new DeleteShip.Command { Ship = ship });
                }

                //delete all shipWrappers
                foreach (var shipWrapper in shipWrappers)
                {
                    var newShipWrapper = _shipWrapperService.GetShipWrapper(shipWrapper.Id);
                    await Mediator.Send(new DeleteShipWrapper.Command { ShipWrapper = newShipWrapper });
                }

                //delete game from table Game
                var game = _gameService.GetGame(gameId);
                await Mediator.Send(new DeleteGame.Command { Game = game });

                //delete field from table Field
                var field = _fieldService.GetField(fieldId);
                await Mediator.Send(new DeleteField.Command { Field = field });

                //update appUser
                var appUser = _appUserService.CreateNewAppUser(firstPlayerId, null);
                await Mediator.Send(new UpdateAppUser.Command { AppUser = appUser });

                return Ok();
            }
            else
            {
                //delete game from table Game
                var game = _gameService.GetGame(gameId);
                await Mediator.Send(new DeleteGame.Command { Game = game });

                //delete field from table Field
                var field = _fieldService.GetField(fieldId);
                await Mediator.Send(new DeleteField.Command { Field = field });

                //update appUser
                var appUser = _appUserService.CreateNewAppUser(firstPlayerId, null);
                await Mediator.Send(new UpdateAppUser.Command { AppUser = appUser });

                return Ok();
            }
        }

        [HttpGet("joinSecondPlayer")]
        public async Task<IActionResult> JoinSecondPlayer(int gameId, string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;
            var secondPlayerId = _playerService.GetPlayerId(username);
            var firstPlayerId = _playerGameService.GetFirstPlayerId(gameId);
            var playerGame = _playerGameService.GetPlayerGame(firstPlayerId, null);

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
            var newPlayerGame = _playerGameService.UpdatePlayerGame(playerGame, secondPlayerId);
            await Mediator.Send(new UpdatePlayerGame.Command { PlayerGame = newPlayerGame });

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
            try
            {
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
            }
            catch (Exception ex)
            {
                await Mediator.Send(new DeleteShipWrapper.Command { ShipWrapper = shipWrapper });
                await Mediator.Send(new DeleteShip.Command { Ship = ship });
                return BadRequest(ex.Message);
            }
            return Ok();
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
            if (shipWrappers.Count() < 11)
            {
                return BadRequest("Number of ships must be 10!");
            }
            var secondPlayerId = _playerGameService.GetSecondPlayerId(firstPlayerId);

            if (secondPlayerId == null)
            {
                var playerGame = _playerGameService.GetPlayerGame(firstPlayerId, null);
                var newPlayerGame = _playerGameService.CreateNewPlayerGame(playerGame);

                await Mediator.Send(new UpdatePlayerGame.Command { PlayerGame = newPlayerGame });

                return Ok("Player is ready!");
            }
            else
            {
                var playerGame = _playerGameService.GetPlayerGame(firstPlayerId, secondPlayerId);
                var newPlayerGame = _playerGameService.CreateNewPlayerGame(playerGame);

                await Mediator.Send(new UpdatePlayerGame.Command { PlayerGame = newPlayerGame });

                return Ok("Player is ready!");
            }
        }

        [HttpGet("isTwoPlayersReady")]
        public async Task<ActionResult<IsReadyViewModel>> IsTwoPlayersReady(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;
            var firstPlayerId = _playerService.GetPlayerId(username);
            var secondPlayerId = _playerGameService.GetSecondPlayerId(firstPlayerId);

            if (secondPlayerId == null)
            {
                var playerGame = _playerGameService.GetPlayerGame(firstPlayerId, null);
                var numberOfReadyPlayers = _playerGameService.GetNumberOfReadyPlayers(playerGame);
                var isReadyViewModel = new IsReadyViewModel { NumberOfReadyPlayers = numberOfReadyPlayers };
                return Ok(isReadyViewModel);
            }
            else
            {
                var playerGame = _playerGameService.GetPlayerGame(firstPlayerId, secondPlayerId);
                var numberOfReadyPlayers = _playerGameService.GetNumberOfReadyPlayers(playerGame);
                var isReadyViewModel = new IsReadyViewModel { NumberOfReadyPlayers = numberOfReadyPlayers };
                return Ok(isReadyViewModel);
            }
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

        [HttpGet("game/endOfTheGame")]
        public async Task<ActionResult<IsEndOfTheGameViewModel>> IsEndOfTheGame(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;
            var firstPlayerId = _playerService.GetPlayerId(username);
            var firstFieldId = _fieldService.GetFieldId(firstPlayerId);
            var firstShipWrappers = _shipWrapperService.GetAllShipWrappersByFiedlId(firstFieldId);
            var firstPositions = _positionService.GetAllPoitionsByShipWrapperId(firstShipWrappers);
            var firstCellsIds = _cellService.GetAllCellsIdByPositions(firstPositions);
            var firstCellList = _cellService.GetAllCellsByCellIds(firstCellsIds);

            var secondPlayerId = _playerGameService.GetSecondPlayerId(firstPlayerId);
            if (secondPlayerId == null)
            {
                var gameId = _playerGameService.GetPlayerGame(firstPlayerId, null).GameId;
                var firstCellsWithStateBusyOrHit = _cellService.CheckIsCellsWithStateBusyOrHit(firstCellList, gameId);
                var secondIsCellsWithStateBusyOrHit = _cellService.CheckIsCellsWithStateBusyOrHit(null, gameId);

                if (secondIsCellsWithStateBusyOrHit && firstCellsWithStateBusyOrHit && _gameService.GetGame(gameId).GameStateId == 2)
                {
                    return Ok(new IsEndOfTheGameViewModel { IsEndOfTheGame = false, WinnerUserName = "" });
                }
                var game = _gameService.GetNewGame(gameId);
                if (!firstCellsWithStateBusyOrHit)
                {
                    await Mediator.Send(new UpdateGame.Command { Game = game });

                    if (_gameHistoryService.CheckIfExistGameHistoryByGameId(gameId) == false)
                    {
                        var gameHistory = _gameHistoryService.CreateGameHistory(gameId, _appUserService.GetUsername(firstPlayerId), _appUserService.GetUsername(secondPlayerId), _gameStateService.GetGameState(3), _appUserService.GetUsername(secondPlayerId));
                        await Mediator.Send(new CreateGameHistory.Command { GameHistory = gameHistory });
                    }

                    await Mediator.Send(new UpdateGame.Command { Game = game });
                    return Ok(new IsEndOfTheGameViewModel { IsEndOfTheGame = true, WinnerUserName = _appUserService.GetUsername(secondPlayerId) });
                }

                else if (!secondIsCellsWithStateBusyOrHit)
                {
                    await Mediator.Send(new UpdateGame.Command { Game = game });

                    if (_gameHistoryService.CheckIfExistGameHistoryByGameId(gameId) == false)
                    {
                        var gameHistory = _gameHistoryService.CreateGameHistory(gameId, _appUserService.GetUsername(firstPlayerId), _appUserService.GetUsername(secondPlayerId), _gameStateService.GetGameState(3), _appUserService.GetUsername(firstPlayerId));
                        await Mediator.Send(new CreateGameHistory.Command { GameHistory = gameHistory });
                    }

                    await Mediator.Send(new UpdateGame.Command { Game = game });
                    return Ok(new IsEndOfTheGameViewModel { IsEndOfTheGame = true, WinnerUserName = username });
                }
                return Ok(new IsEndOfTheGameViewModel { IsEndOfTheGame = false, WinnerUserName = "" });
            }
            else
            {
                var gameId = _playerGameService.GetPlayerGame(firstPlayerId, secondPlayerId).GameId;
                var secondFieldId = _fieldService.GetFieldId(secondPlayerId);
                var secondShipWrappers = _shipWrapperService.GetAllShipWrappersByFiedlId(secondFieldId);
                var secondPositions = _positionService.GetAllPoitionsByShipWrapperId(secondShipWrappers);
                var secondCellsIds = _cellService.GetAllCellsIdByPositions(secondPositions);
                var secondCellList = _cellService.GetAllCellsByCellIds(secondCellsIds);

                var firstCellsWithStateBusyOrHit = _cellService.CheckIsCellsWithStateBusyOrHit(firstCellList, _gameService.GetGame(gameId).GameStateId);
                var secondCellsWithStateBusyOrHit = _cellService.CheckIsCellsWithStateBusyOrHit(secondCellList, _gameService.GetGame(gameId).GameStateId);

                if (secondCellsWithStateBusyOrHit && firstCellsWithStateBusyOrHit && _gameService.GetGame(gameId).GameStateId == 2)
                {
                    return Ok(new IsEndOfTheGameViewModel { IsEndOfTheGame = false, WinnerUserName = "" });
                }

                var game = _gameService.GetNewGame(gameId);
                if (!firstCellsWithStateBusyOrHit)
                {
                    await Mediator.Send(new UpdateGame.Command { Game = game });

                    if (_gameHistoryService.CheckIfExistGameHistoryByGameId(gameId) == false)
                    {
                        var gameHistory = _gameHistoryService.CreateGameHistory(gameId, _appUserService.GetUsername(firstPlayerId), _appUserService.GetUsername(secondPlayerId), _gameStateService.GetGameState(3), _appUserService.GetUsername(secondPlayerId));
                        await Mediator.Send(new CreateGameHistory.Command { GameHistory = gameHistory });
                    }

                    await Mediator.Send(new UpdateGame.Command { Game = game });
                    return Ok(new IsEndOfTheGameViewModel { IsEndOfTheGame = true, WinnerUserName = _appUserService.GetUsername(secondPlayerId) });
                }
                else if (!secondCellsWithStateBusyOrHit)
                {
                    await Mediator.Send(new UpdateGame.Command { Game = game });

                    if (_gameHistoryService.CheckIfExistGameHistoryByGameId(gameId) == false)
                    {
                        var gameHistory = _gameHistoryService.CreateGameHistory(gameId, _appUserService.GetUsername(firstPlayerId), _appUserService.GetUsername(secondPlayerId), _gameStateService.GetGameState(3), _appUserService.GetUsername(firstPlayerId));
                        await Mediator.Send(new CreateGameHistory.Command { GameHistory = gameHistory });
                    }

                    await Mediator.Send(new UpdateGame.Command { Game = game });
                    return Ok(new IsEndOfTheGameViewModel { IsEndOfTheGame = true, WinnerUserName = username });
                }
                return Ok(new IsEndOfTheGameViewModel { IsEndOfTheGame = false, WinnerUserName = "" });
            }
        }

        [HttpGet("game/clearingDb")]
        public async Task<IActionResult> ClearingDB(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var username = jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value;
                var firstPlayerId = _playerService.GetPlayerId(username);
                var secondPlayerId = _playerGameService.GetSecondPlayerId(firstPlayerId);

                var firstFieldId = _fieldService.GetFieldId(firstPlayerId);
                var firstFieldShipIds = _shipWrapperService.GetAllShipIdsByFieldId(firstFieldId);
                var firstShips = _shipService.GetAllShips(firstFieldShipIds);
                var firstShipWrappers = _shipWrapperService.GetAllShipWrappersByFiedlId(firstFieldId);
                var firstPositions = _positionService.GetAllPoitionsByShipWrapperId(firstShipWrappers);
                var firstCellsIds = _cellService.GetAllCellsIdByPositions(firstPositions);
                var firstCellList = _cellService.GetAllCellsByCellIds(firstCellsIds);

                var secondFieldId = _fieldService.GetFieldId(secondPlayerId);
                var secondFieldShipIds = _shipWrapperService.GetAllShipIdsByFieldId(secondFieldId);
                var secondShips = _shipService.GetAllShips(secondFieldShipIds);
                var secondShipWrappers = _shipWrapperService.GetAllShipWrappersByFiedlId(secondFieldId);
                var secondPositions = _positionService.GetAllPoitionsByShipWrapperId(secondShipWrappers);


                if (secondPositions.Any())
                {
                    //delete all cells
                    foreach (var cell in firstCellList)
                    {
                        await Mediator.Send(new DeleteCell.Command { Cell = cell });
                    }

                    //delete all ships
                    foreach (var ship in firstShips)
                    {
                        await Mediator.Send(new DeleteShip.Command { Ship = ship });
                    }

                    //update table AppUser(cleanup column IsHit)
                    var firstAppUser = _appUserService.CreateNewAppUser(firstPlayerId, null);
                    await Mediator.Send(new UpdateAppUser.Command { AppUser = firstAppUser });
                    var secondAppUser = _appUserService.CreateNewAppUser(secondPlayerId, null);
                    await Mediator.Send(new UpdateAppUser.Command { AppUser = secondAppUser });

                    return Ok("The database cleanup was successfull!");
                }
                else
                {
                    //delete all cells
                    foreach (var cell in firstCellList)
                    {
                        await Mediator.Send(new DeleteCell.Command { Cell = cell });
                    }

                    //delete all ships
                    foreach (var ship in firstShips)
                    {
                        await Mediator.Send(new DeleteShip.Command { Ship = ship });
                    }

                    //delete all shipWrappers
                    foreach (var shipWrapper in firstShipWrappers)
                    {
                        var newShipWrapper = _shipWrapperService.GetShipWrapper(shipWrapper.Id);
                        await Mediator.Send(new DeleteShipWrapper.Command { ShipWrapper = newShipWrapper });
                    }
                    //delete all shipWrappers
                    foreach (var shipWrapper in secondShipWrappers)
                    {
                        var newShipWrapper = _shipWrapperService.GetShipWrapper(shipWrapper.Id);
                        await Mediator.Send(new DeleteShipWrapper.Command { ShipWrapper = newShipWrapper });
                    }

                    //delete game form table Game
                    var gameId = _playerGameService.GetPlayerGame(firstPlayerId, secondPlayerId).GameId;
                    var game = _gameService.GetGame(gameId);
                    await Mediator.Send(new DeleteGame.Command { Game = game });

                    //delete field from table Field
                    var field = _fieldService.GetField(firstFieldId);
                    await Mediator.Send(new DeleteField.Command { Field = field });

                    //delete field from table Field
                    var secondField = _fieldService.GetField(secondFieldId);
                    await Mediator.Send(new DeleteField.Command { Field = secondField });

                    return Ok("The database cleanup was successfull!");
                }
            }
            catch(Exception ex)
            {
                return BadRequest($"The database cleanup failed! Error message - {ex.Message}");
            }
        }
    }
}