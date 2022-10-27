using DAL.Models;

namespace BLL.Interfaces
{
    public interface IFieldService
    {
        Field CreateField(int size, string firstPlayerId);
        int GetFieldId(string playerId);
    }
}
