using DAL.Models;

namespace BLL.Interfaces
{
    public interface ICellService
    {
        bool IsCellBusy(int x, int y);
        List<Cell> getAllCells(string shipDirectionName, int shipSize, int x, int y);
    }
}
