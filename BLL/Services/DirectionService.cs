using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class DirectionService : IDirectionService
    {
        private readonly IRepository<Direction> _repository;

        public DirectionService(IRepository<Direction> repository)
        {
            _repository = repository;
        }

        public string GetDirectionName(int id)
        {
            return _repository.GetById(id).Result.DirectionName;
        }
    }
}
