using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class PositionService : IPositionService
    {
        private readonly IRepository<Position> _repository;

        public PositionService(IRepository<Position> repository)
        {
            _repository = repository;
        }

        public List<Position> GetAllPositions(int shipWrapperId, List<Cell> cells)
        {
            var positionList = new List<Position>();

            foreach(var cell in cells)
            {
                positionList.Add(new Position { ShipWrapperId = shipWrapperId, CellId = cell.Id });
            }

            return positionList;
        }
    }
}
