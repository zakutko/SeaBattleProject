using DAL.Models;

namespace BLL.Interfaces
{
    public interface ICellService
    {
        Cell UpdateCell(int cellId, int x, int y, int cellStateId);
        IEnumerable<Cell> GetAllCells(string shipDirectionName, int shipSize, int x, int y, int fieldId);
        IEnumerable<int> GetAllCellsIdByPositions(IEnumerable<Position> positions);
        IEnumerable<Cell> GetAllCellsByCellIds(IEnumerable<int> cellIds);
        IEnumerable<Cell> SetDefaultCells();
        int GetCellId(int X, int Y, IEnumerable<ShipWrapper> shipWrappers);
        Cell CreateNewCell(int id, int x, int y, int cellStateId, bool isDestroyed);
        Cell GetCell(int x, int y, IEnumerable<Cell> cells);
        bool CheckIfTheShipIsDestroyed(IEnumerable<Cell> cells);
    }
}
