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

        public int GetCellId(int x, int y, IEnumerable<ShipWrapper> shipWrappers)
        {
            var positions = _positionRepository.GetByShipWrapperId(shipWrappers);
            var cellIds = _positionRepository.GetAllCellIds(positions);
            var cells = new List<Cell>();

            foreach (var cellId in cellIds)
            {
                cells.Add(_repository.GetById(cellId).Result);
            }

            var ids = new List<int>();
            foreach (var cell in cells)
            {
                if (cell.X == x && cell.Y == y)
                {
                    ids.Add(cell.Id);
                }
            }
            return ids.First();
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
                cellList.Add(_repository.GetById(cellId).Result);
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

            var shipWrappers = _shipWrapperRepository.GetAll().Result.Where(x => x.FieldId == fieldId && x.ShipId != null);

            var positions = _positionRepository.GetByShipWrapperId(shipWrappers);

            var cellIds = _positionRepository.GetAllCellIds(positions);

            var cells = new List<Cell>();

            foreach (var cellId in cellIds)
            {
                cells.Add(_repository.GetById(cellId).Result);
            }

            for (int i = 0; i < shipSize; i++)
            {
                if (shipDirectionName == "Vertical")
                {
                    var newCell = new Cell { X = x, Y = y + i, CellStateId = 2 };

                    var aroundNewCells = new List<Cell>();
                    if (i == 0)
                    {
                        switch (shipSize)
                        {
                            case 1:
                                aroundNewCells.AddRange(new List<Cell> {
                                    new Cell { X = newCell.X, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y, CellStateId = 5},
                                    new Cell { X = newCell.X, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y + 1, CellStateId = 5},
                                });
                                break;
                            case 2:
                                aroundNewCells.AddRange(new List<Cell> {
                                    new Cell { X = newCell.X, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y + 2, CellStateId = 5},
                                    new Cell { X = newCell.X, Y = newCell.Y + 2, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y + 2, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y - 1, CellStateId = 5},
                                });
                                break;
                            case 3:
                                aroundNewCells.AddRange(new List<Cell> {
                                    new Cell { X = newCell.X, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y + 2, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y + 3, CellStateId = 5},
                                    new Cell { X = newCell.X, Y = newCell.Y + 3, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y + 3, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y + 2, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y - 1, CellStateId = 5},
                                });
                                break;
                            case 4:
                                aroundNewCells.AddRange(new List<Cell> {
                                    new Cell { X = newCell.X, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y + 2, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y + 3, CellStateId = 5},

                                    new Cell { X = newCell.X + 1, Y = newCell.Y + 4, CellStateId = 5},
                                    new Cell { X = newCell.X, Y = newCell.Y + 4, CellStateId = 5},

                                    new Cell { X = newCell.X - 1, Y = newCell.Y + 4, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y + 3, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y + 2, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y - 1, CellStateId = 5},
                                });
                                break;
                        };
                    }

                    var newArroundList = new List<Cell>();
                    foreach (var arroundNewCell in aroundNewCells)
                    {
                        if (!(arroundNewCell.X < 1 || arroundNewCell.X > 10 || arroundNewCell.Y < 1 || arroundNewCell.Y > 10))
                        {
                            newArroundList.Add(arroundNewCell);
                        }
                    }

                    if (!cells.Any())
                    {
                        newCells.Add(newCell);
                        newCells.AddRange(newArroundList);
                    }

                    else
                    {
                        var allCellsWithStateNoEmpty = cells.Where(x => x.CellStateId == 2 || x.CellStateId == 5);
                        var isMatches = false;
                        foreach (var cell in allCellsWithStateNoEmpty)
                        {
                            if (newCell.X == cell.X && newCell.Y == cell.Y)
                            {
                                isMatches = true;
                                break;
                            }
                        }
                        if (isMatches)
                        {
                            aroundNewCells.Clear();
                            newCells.Clear();
                        }
                        else
                        {
                            newCells.AddRange(newArroundList);
                            newCells.Add(newCell);
                        }
                    }
                }

                else if (shipDirectionName == "Horizontal")
                {
                    var newCell = new Cell { X = x + i, Y = y, CellStateId = 2 };
                    var aroundNewCells = new List<Cell>();
                    if (i == 0)
                    {
                        switch (shipSize)
                        {
                            case 1:
                                aroundNewCells.AddRange(new List<Cell> {
                                    new Cell { X = newCell.X, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y, CellStateId = 5},
                                    new Cell { X = newCell.X, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y + 1, CellStateId = 5},
                                });
                                break;
                            case 2:
                                aroundNewCells.AddRange(new List<Cell> {
                                    new Cell { X = newCell.X - 1, Y = newCell.Y, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 2, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 2, Y = newCell.Y, CellStateId = 5},
                                    new Cell { X = newCell.X + 2, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y + 1, CellStateId = 5},
                                });
                                break;
                            case 3:
                                aroundNewCells.AddRange(new List<Cell> {
                                    new Cell { X = newCell.X - 1, Y = newCell.Y, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 2, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 3, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 3, Y = newCell.Y, CellStateId = 5},
                                    new Cell { X = newCell.X + 3, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 2, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y + 1, CellStateId = 5},
                                });
                                break;
                            case 4:
                                aroundNewCells.AddRange(new List<Cell> {
                                    new Cell { X = newCell.X - 1, Y = newCell.Y, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 2, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 3, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 4, Y = newCell.Y - 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 4, Y = newCell.Y, CellStateId = 5},
                                    new Cell { X = newCell.X + 4, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 3, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 2, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X + 1, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X, Y = newCell.Y + 1, CellStateId = 5},
                                    new Cell { X = newCell.X - 1, Y = newCell.Y + 1, CellStateId = 5},
                                });
                                break;
                        };
                    }

                    var newArroundList = new List<Cell>();
                    foreach (var arroundNewCell in aroundNewCells)
                    {
                        if (arroundNewCell.X < 1 || arroundNewCell.X > 10 || arroundNewCell.Y < 1 || arroundNewCell.Y > 10)
                        {
                            continue;
                        }
                        newArroundList.Add(arroundNewCell);
                    }

                    if (!cells.Any())
                    {
                        newCells.Add(newCell);
                        newCells.AddRange(newArroundList);
                    }
                    else
                    {
                        var allCellsWithStateNoEmpty = cells.Where(x => x.CellStateId == 2 || x.CellStateId == 5);
                        var isMatches = false;
                        foreach (var cell in allCellsWithStateNoEmpty)
                        {
                            if (newCell.X == cell.X && newCell.Y == cell.Y)
                            {
                                isMatches = true;
                                break;
                            }
                        }
                        if (isMatches)
                        {
                            aroundNewCells.Clear();
                            newCells.Clear();
                        }
                        else
                        {
                            newCells.AddRange(newArroundList);
                            newCells.Add(newCell);
                        }
                    }
                }
            }
            return newCells;
        }
    }
}
