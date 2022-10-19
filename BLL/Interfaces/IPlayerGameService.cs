namespace BLL.Interfaces
{
    public interface IPlayerGameService
    {
        int GetNumberOfPlayers(int id);
        string GetFirstPlayerId(int id);
        int GetPlayerGameId(int id, string firstPlayerId);
    }
}
