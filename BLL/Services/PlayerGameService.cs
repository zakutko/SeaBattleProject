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

        public string GetSecondPlayerId(string firstPlayerId)
        {
            var playerGame = _repository.GetAll().Result.Where(x => x.FirstPlayerId == firstPlayerId && x.SecondPlayerId != null || x.SecondPlayerId == firstPlayerId && x.FirstPlayerId != null).FirstOrDefault();

            if (playerGame == null) 
            {
                throw new Exception("The second player has not connected yet!");
            }
            if (playerGame.FirstPlayerId == firstPlayerId)
            {
                return playerGame.SecondPlayerId;
            }
            return playerGame.FirstPlayerId;
        }
        
        public int GetNumberOfReadyPlayers(string firstPlayerId, string secondPlayerId)
        {
            var playerGame = _repository.GetAll().Result.Where(x => 
            x.FirstPlayerId == firstPlayerId && x.SecondPlayerId == secondPlayerId ||
            x.FirstPlayerId == secondPlayerId && x.SecondPlayerId == firstPlayerId
            ).FirstOrDefault();

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

        public PlayerGame GetPlayerGame(string firstPlayerId, string secondPlayerId)
        {
            return _repository.GetAll().Result.Where(x => 
                x.FirstPlayerId == firstPlayerId && x.SecondPlayerId == secondPlayerId ||
                x.FirstPlayerId == secondPlayerId && x.SecondPlayerId == firstPlayerId
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
    }
}
