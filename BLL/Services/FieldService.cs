using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class FieldService : IFieldService
    {
        private readonly IRepository<Field> _repository;

        public FieldService(IRepository<Field> repository)
        {
            _repository = repository;
        }

        public int GetFieldId(string playerId)
        {
            return _repository.GetAll().Where(x => x.PlayerId == playerId).FirstOrDefault().Id;
        }
    }
}
