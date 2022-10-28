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

        public Field CreateField(int size, string playerId)
        {
            return new Field { Size = 10, PlayerId = playerId };
        }

        public int GetFieldId(string playerId)
        {
            return _repository.GetAll().Result.Where(x => x.PlayerId == playerId).FirstOrDefault().Id;
        }

        public Field GetField(int fieldId)
        {
            return _repository.GetById(fieldId).Result;
        }
    }
}
