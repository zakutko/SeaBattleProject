using DAL.Models;

namespace BLL.Interfaces
{
    public interface IPositionService
    {
        List<Position> GetAllPositions(int shipWrapperId, List<Cell> cells);
    }
}
