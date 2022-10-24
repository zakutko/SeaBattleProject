using DAL.Models;

namespace DAL.Interfaces
{
    public interface IPositionRepository : IRepository<Position>
    {
        IEnumerable<Position> GetByShipWrapperId(IEnumerable<ShipWrapper> shipWrappers);
        IEnumerable<int> GetAllCellIds(IEnumerable<Position> positions);
    }
}
