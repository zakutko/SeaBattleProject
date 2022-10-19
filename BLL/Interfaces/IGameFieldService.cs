namespace BLL.Interfaces
{
    public interface IGameFieldService
    {
        int GetFirstFieldId(int id);
        int GetGameFieldId(int id, int firstFieldId);
    }
}
