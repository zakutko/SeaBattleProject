using DAL.Models;

namespace BLL.Interfaces
{
    public interface IPositionService
    {
        IEnumerable<Position> GetAllPositions(int shipWrapperId, IEnumerable<Cell> cells);
        IEnumerable<Position> GetAllPoitionsByShipWrapperId(IEnumerable<ShipWrapper> shipWrappers);
        IEnumerable<Position> GetAllPoitionsByShipWrapperId(int shipWrapperId);
        IEnumerable<Position> SetDefaultPositions(int shipWrapperId, IEnumerable<Cell> cells);
        int GetPositionId(int shipWrapperId);
        int GetPositionByCellId(int cellId);
        int GetShipWrapperIdByCellId(int cellId);
        IEnumerable<int> GetAllCellIdsByPositions(IEnumerable<Position> cells);
    }
}
