using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IRepository<AppUser> _repository;

        public PlayerService(IRepository<AppUser> repository)
        {
            _repository = repository;
        }
        public AppUser GetPlayer(string id)
        {
            return _repository.GetById(id).Result;
        }
    }
}
