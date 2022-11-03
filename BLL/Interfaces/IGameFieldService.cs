using DAL.Models;

namespace BLL.Interfaces
{
    public interface IGameFieldService
    {
        GameField CreateGameField(int fieldId, int gameId);
        GameField UpdateGameField(int gameFieldId, int firstFieldId, int secondFieldId, int gameId);
        int GetFirstFieldId(int id);
        int GetGameFieldId(int id, int firstFieldId);
    }
}
