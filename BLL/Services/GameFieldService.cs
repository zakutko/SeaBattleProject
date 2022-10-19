using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class GameFieldService : IGameFieldService
    {
        private readonly IRepository<GameField> _repository;

        public GameFieldService(IRepository<GameField> repository)
        {
            _repository = repository;
        }

        public int GetFirstFieldId(int id)
        {
            var gameField = _repository.GetById(id);
            var firstFieldId = gameField.FirstFieldId;

            return (int)firstFieldId;
        }

        public int GetGameFieldId(int id, int firstField)
        {
            var gameField = _repository.GetAll().Where(x => x.GameId == id && x.FirstFieldId == firstField);
            var gameFieldId = gameField.FirstOrDefault().Id;

            return gameFieldId;
        }
    }
}
