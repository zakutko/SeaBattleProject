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

        public Position UpdatePosition(int positionId, int shipWrapperId, int cellId)
        {
            return new Position { Id = positionId, ShipWrapperId = shipWrapperId, CellId = cellId };
        }

        public int GetPositionId(int shipWrapperId)
        {
            return _repository.GetAll().Result.Where(x => x.ShipWrapperId == shipWrapperId).FirstOrDefault().Id;
        }

        public int GetPositionByCellId(int cellId)
        {
            return _repository.GetAll().Result.Where(x => x.CellId == cellId).FirstOrDefault().Id;
        }

        public int GetShipWrapperIdByCellId(int cellId)
        {
            var positions = _repository.GetAll().Result.Where(x => x.CellId == cellId);
            return positions.FirstOrDefault().ShipWrapperId;
        }

        public IEnumerable<Position> SetDefaultPositions(int shipWrapperId, IEnumerable<Cell> cells)
        {
            var positions = new List<Position>();

            foreach (var cell in cells)
            {
                positions.Add(new Position { ShipWrapperId = shipWrapperId, CellId = cell.Id });
            }

            return positions;
        }

        public IEnumerable<Position> GetAllPositions(int shipWrapperId, IEnumerable<Cell> cells)
        {
            var positionList = new List<Position>();

            foreach(var cell in cells)
            {
                positionList.Add(new Position { ShipWrapperId = shipWrapperId, CellId = cell.Id });
            }

            return positionList;
        }

        public IEnumerable<Position> GetAllPoitionsByShipWrapperId(IEnumerable<ShipWrapper> shipWrappers)
        {
            var positions = new List<Position>();

            foreach (var shipWrapper in shipWrappers)
            {
                positions.AddRange(_repository.GetAll().Result.Where(x => x.ShipWrapperId == shipWrapper.Id));
            }

            return positions;
        }

        public IEnumerable<Position> GetAllPoitionsByShipWrapperId(int shipWrapperId)
        {
            return _repository.GetAll().Result.Where(x => x.ShipWrapperId == shipWrapperId);
        }

        public IEnumerable<int> GetAllCellIdsByPositions(IEnumerable<Position> positions)
        {
            var cellIds = new List<int>();

            foreach (var position in positions)
            {
                cellIds.Add(_repository.GetById(position.Id).Result.CellId);
            }

            return cellIds;
        }
    }
}
