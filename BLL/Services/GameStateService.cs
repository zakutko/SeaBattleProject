using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class GameStateService : IGameStateService
    {
        private readonly IRepository<GameState> _repository;

        public GameStateService(IRepository<GameState> repository)
        {
            _repository = repository;
        }

        public GameState GetGameState(int id)
        {
            var result = _repository.GetById(id);

            if (result == null)
            {
                throw new Exception("GameState does not exist!");
            }

            return result;
        }
    }
}
