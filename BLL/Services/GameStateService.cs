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

        public string GetGameState(int id)
        {
            var gameState = _repository.GetById(id);

            return gameState.GameStateName;
        }
    }
}
