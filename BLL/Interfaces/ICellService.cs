using DAL.Models;

namespace BLL.Interfaces
{
    public interface ICellService
    {
        IEnumerable<Cell> GetAllCells(string shipDirectionName, int shipSize, int x, int y, int fieldId);
        IEnumerable<int> GetAllCellsIdByPositions(IEnumerable<Position> positions);
        IEnumerable<Cell> GetAllCellsByCellIds(IEnumerable<int> cellIds);
        IEnumerable<Cell> SetDefaultCells();
        int GetCellId(int X, int Y, IEnumerable<ShipWrapper> shipWrappers);
    }
}
