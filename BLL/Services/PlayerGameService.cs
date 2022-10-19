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
            var playerGame = _repository.GetAll().Where(x => x.GameId == id).FirstOrDefault();
            var numberOfPlayers = 1;

            if (playerGame.SecondPlayerId != null)
            {
                numberOfPlayers = 2;
            }

            return numberOfPlayers;
        }

        public string GetFirstPlayerId(int id)
        {
            var playerGame = _repository.GetAll().Where(x => x.GameId == id).FirstOrDefault();
            var firstPlayerId = playerGame.FirstPlayerId;

            return firstPlayerId;
        }

        public int GetPlayerGameId(int id, string firstPlayerId)
        {
            var playerGame = _repository.GetAll().Where(x => x.GameId == id && x.FirstPlayerId == firstPlayerId);
            var playerGameId = playerGame.FirstOrDefault().Id;

            return playerGameId;
        }
    }
}
