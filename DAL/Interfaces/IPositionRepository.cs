using DAL.Models;

namespace DAL.Interfaces
{
    public interface IPositionRepository
    {
        IEnumerable<Position> GetByShipWrapperId(IEnumerable<ShipWrapper> shipWrappers);
        IEnumerable<int> GetAllCellIds(IEnumerable<Position> positions);
    }
}
