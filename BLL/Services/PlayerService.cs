using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IAppUserRepository _repository;

        public PlayerService(IAppUserRepository repository)
        {
            _repository = repository;
        }
        public AppUser GetPlayer(string id)
        {
            return _repository.GetById(id).Result;
        }
        public string GetPlayerId(string username)
        {
            var player = _repository.GetByUsername(username);

            return player.Id;
        }
    }
}
