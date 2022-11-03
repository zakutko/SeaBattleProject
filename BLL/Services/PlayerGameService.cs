using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class PlayerGameService : IPlayerGameService
    {
        private readonly IRepository<PlayerGame> _repository;

        public PlayerGameService(IRepository<PlayerGame> repository)
        {
            _repository = repository;
        }

        public PlayerGame CreatePlayerGame(int gameId, string playerId)
        {
            return new PlayerGame { GameId = gameId, FirstPlayerId = playerId };
        }

        public PlayerGame UpdatePlayerGame(PlayerGame playerGame, string secondPlayerId)
        {
            return new PlayerGame { 
                Id = playerGame.Id, 
                GameId = playerGame.GameId, 
                FirstPlayerId = playerGame.FirstPlayerId, 
                SecondPlayerId = secondPlayerId, 
                IsReadyFirstPlayer = playerGame.IsReadyFirstPlayer,
                IsReadySecondPlayer = playerGame.IsReadySecondPlayer
            };
        }

        public int GetNumberOfPlayers(int id)
        {
            var playerGame = _repository.GetAll().Result.Where(x => x.GameId == id).FirstOrDefault();
            var numberOfPlayers = 1;

            if (playerGame.SecondPlayerId != null)
            {
                numberOfPlayers = 2;
            }

            return numberOfPlayers;
        }

        public string GetFirstPlayerId(int id)
        {
            var playerGame = _repository.GetAll().Result.Where(x => x.GameId == id).FirstOrDefault();
            var firstPlayerId = playerGame.FirstPlayerId;

            return firstPlayerId;
        }

        public int GetPlayerGameId(int id, string firstPlayerId)
        {
            var playerGame = _repository.GetAll().Result.Where(x => x.GameId == id && x.FirstPlayerId == firstPlayerId);
            var playerGameId = playerGame.FirstOrDefault().Id;

            return playerGameId;
        }

        public string? GetSecondPlayerId(string firstPlayerId)
        {
            var playerGame = _repository.GetAll().Result.Where(x => x.FirstPlayerId == firstPlayerId && x.SecondPlayerId != null || x.SecondPlayerId == firstPlayerId && x.FirstPlayerId != null).FirstOrDefault();

            if (playerGame == null) 
            {
                return null;
            }
            if (playerGame.FirstPlayerId == firstPlayerId)
            {
                return playerGame.SecondPlayerId;
            }
            return playerGame.FirstPlayerId;
        }
        
        public int GetNumberOfReadyPlayers(PlayerGame playerGame)
        {
            if (playerGame.IsReadyFirstPlayer == null && playerGame.IsReadySecondPlayer == null)
            {
                return 0;
            }
            else if (playerGame.IsReadyFirstPlayer != null && playerGame.IsReadySecondPlayer == null || playerGame.IsReadyFirstPlayer == null && playerGame.IsReadySecondPlayer != null)
            {
                return 1;
            }
            return 2;
        }

        public PlayerGame GetPlayerGame(string firstPlayerId, string? secondPlayerId)
        {
            return _repository.GetAll().Result.Where(x => 
                x.FirstPlayerId == firstPlayerId && x.SecondPlayerId == secondPlayerId ||
                x.FirstPlayerId == secondPlayerId && x.SecondPlayerId == firstPlayerId ||
                x.FirstPlayerId == firstPlayerId && x.SecondPlayerId == null ||
                x.FirstPlayerId == secondPlayerId && x.SecondPlayerId == null
                ).FirstOrDefault();   
        }

        public PlayerGame CreateNewPlayerGame(PlayerGame playerGame)
        {
            if (playerGame.IsReadyFirstPlayer == null && playerGame.IsReadySecondPlayer == null)
            {
                return new PlayerGame
                {
                    Id = playerGame.Id,
                    GameId = playerGame.GameId,
                    FirstPlayerId = playerGame.FirstPlayerId,
                    SecondPlayerId = playerGame.SecondPlayerId,
                    IsReadyFirstPlayer = true,
                    IsReadySecondPlayer = playerGame.IsReadySecondPlayer
                };
            }
            else
            {
                return new PlayerGame
                {
                    Id = playerGame.Id,
                    GameId = playerGame.GameId,
                    FirstPlayerId = playerGame.FirstPlayerId,
                    SecondPlayerId = playerGame.SecondPlayerId,
                    IsReadyFirstPlayer = true,
                    IsReadySecondPlayer = true
                };
            }
        }

        public bool IsGameOwner(string playerId)
        {
            var playerGame = _repository.GetAll().Result.Where(x => x.FirstPlayerId == playerId || x.SecondPlayerId == playerId).FirstOrDefault();

            if (playerGame.FirstPlayerId == playerId)
            {
                return true;
            }
            return false;
        }

        public bool IsSecondPlayerConnected(string playerId)
        {
            var playerGame = _repository.GetAll().Result.Where(x => x.FirstPlayerId == playerId || x.SecondPlayerId == playerId).FirstOrDefault();

            if (playerGame.FirstPlayerId == playerId && playerGame.SecondPlayerId == null)
            {
                return false;
            }
            return true;
        }
    }
}