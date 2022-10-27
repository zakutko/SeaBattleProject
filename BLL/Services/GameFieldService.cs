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

        public GameField CreateGameField(int fieldId, int gameId)
        {
            return new GameField { FirstFieldId = fieldId, GameId = gameId };
        }

        public GameField UpdateGameField(int gameFieldId, int firstFieldId, int secondFieldId, int gameId)
        {
            return new GameField { Id = gameFieldId, FirstFieldId = firstFieldId, SecondFieldId = secondFieldId, GameId = gameId };
        }

        public int GetFirstFieldId(int id)
        {
            var gameField = _repository.GetById(id).Result;
            var firstFieldId = gameField.FirstFieldId;

            return (int)firstFieldId;
        }

        public int GetGameFieldId(int id, int firstField)
        {
            var gameField = _repository.GetAll().Result.Where(x => x.GameId == id && x.FirstFieldId == firstField);
            var gameFieldId = gameField.FirstOrDefault().Id;

            return gameFieldId;
        }
    }
}
