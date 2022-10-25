using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IRepository<Game> _repository;
        public GameService(IRepository<Game> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Game> GetGames()
        {
            var result = _repository.GetAll().Result;

            if (!result.Any())
            {
                throw new Exception("No game exist!");
            }
            
            return result;
        }

        public Game GetGame(int id)
        {
            var result = _repository.GetById(id).Result;

            if (result == null) 
            {
                throw new Exception("Game does not exist!");
            }

            return result;
        }
    }
}
