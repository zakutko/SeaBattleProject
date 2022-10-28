using DAL.Models;

namespace BLL.Interfaces
{
    public interface IShipWrapperService
    {
        ShipWrapper GetShipWrapper(int shipWrapperId);
        ShipWrapper CreateShipWrapper(int shipId, int fieldId);
        int GetNumberOfShips(int fieldId);
        int GetNumberOfShipsWhereSizeOne(int fieldId);
        int GetNumberOfShipsWhereSizeTwo(int fieldId);
        int GetNumberOfShipsWhereSizeThree(int fieldId);
        int GetNumberOfShipsWhereSizeFour(int fieldId);
        IEnumerable<ShipWrapper> GetAllShipWrappersByFiedlId(int fieldId);
        ShipWrapper GetDefaultShipWrapper(int fieldId);
        IEnumerable<int> GetAllShipIdsByFieldId(int fieldId);
    }
}
