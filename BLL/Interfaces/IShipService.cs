using DAL.Models;

namespace BLL.Interfaces
{
    public interface IShipService
    {
        Ship CreateShip(int directionId, int shipStateId, int shipSizeId);
        IEnumerable<Ship> GetAllShips(IEnumerable<int> shipIds);
    }
}
