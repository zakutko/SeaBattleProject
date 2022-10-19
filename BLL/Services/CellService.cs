using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class CellService : ICellService
    {
        private readonly IRepository<Cell> _repository;

        public CellService(IRepository<Cell> repository)
        {
            _repository = repository;
        }

        public List<Cell> getAllCells(string shipDirectionName, int shipSize, int x, int y)
        {
            var cells = new List<Cell>();

            for(int i = 0; i < shipSize; i++)
            {
                if (shipDirectionName == "Vertical")
                {
                    var cell = new Cell { X = x, Y = y + i, CellStateId = 2 };

                    if (IsCellBusy(cell.X, cell.Y))
                    {
                        break;
                    }

                    cells.Add(cell);
                }
                else if (shipDirectionName == "Horizontal")
                {
                    var cell = new Cell { X = x + i, Y = y, CellStateId = 2 };

                    if (IsCellBusy(cell.X, cell.Y))
                    {
                        break;
                    }

                    cells.Add(cell);
                }
            }

            return cells;
        }

        public bool IsCellBusy(int x, int y)
        {
            var cell = _repository.GetAll().Where(opt => opt.X == x && opt.Y == y).FirstOrDefault();

            return cell != null;
        }


    }
}
