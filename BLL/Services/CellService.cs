using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class CellService : ICellService
    {
        private readonly IRepository<Cell> _repository;
        private readonly IRepository<ShipWrapper> _shipWrapperRepository;
        private readonly IPositionRepository _positionRepository;

        public CellService(IRepository<Cell> repository, IRepository<ShipWrapper> shipWrapperRepository, IPositionRepository positionRepository)
        {
            _repository = repository;
            _shipWrapperRepository = shipWrapperRepository;
            _positionRepository = positionRepository;
        }

        public int GetCellId(int x, int y)
        {
            return _repository.GetAll().Where(o => o.X == x && o.Y == y).FirstOrDefault().Id;
        }

        public IEnumerable<Cell> SetDefaultCells()
        {
            var cells = new List<Cell>();

            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    cells.Add(new Cell { X = i, Y = j, CellStateId = 1}); 
                }
            }

            return cells;
        }

        public IEnumerable<Cell> GetAllCellsByCellIds(IEnumerable<int> cellIds)
        {
            var cellList = new List<Cell>();

            foreach (var cellId in cellIds)
            {
                cellList.Add(_repository.GetById(cellId));
            }

            return cellList;
        }

        public IEnumerable<int> GetAllCellsIdByPositions(IEnumerable<Position> positions)
        {
            var cellIdList = new List<int>();

            foreach (var position in positions)
            {
                cellIdList.Add(position.CellId);
            }

            return cellIdList;
        }

        public IEnumerable<Cell> GetAllCells(string shipDirectionName, int shipSize, int x, int y, int fieldId)
        {
            var newCells = new List<Cell>();

            var shipWrappers = _shipWrapperRepository.GetAll().Where(x => x.FieldId == fieldId && x.ShipId != null);

            var positions = _positionRepository.GetByShipWrapperId(shipWrappers);

            var cellIds = _positionRepository.GetAllCellIds(positions);

            var cells = new List<Cell>();

            foreach (var cellId in cellIds)
            {
                cells.Add(_repository.GetById(cellId));
            }

            for (int i = 0; i < shipSize; i++)
            {
                if (shipDirectionName == "Vertical")
                {
                    var newCell = new Cell { X = x, Y = y + i, CellStateId = 2 };

                    foreach(var cell in cells)
                    {
                        if (cell.X == newCell.X && cell.Y == newCell.Y)
                        {
                            newCells.Clear();
                            break;
                        }

                        newCells.Add(newCell);
                    }
                }
                else if (shipDirectionName == "Horizontal")
                {
                    var newCell = new Cell { X = x + i, Y = y, CellStateId = 2 };

                    if (!cells.Any())
                    {
                        newCells.Add(newCell);
                    }

                    foreach (var cell in cells)
                    {
                        if (cell.X == newCell.X && cell.Y == newCell.Y)
                        {
                            newCells.Clear();
                            break;
                        }
                        newCells.Add(newCell);
                    }
                }
            }
            return newCells;
        }
    }
}
