namespace WEB.ViewModels
{
    public class GameListViewModel
    {
        public int Id { get; set; }
        public string FirstPlayer { get; set; }
        public string? SecondPlayer { get; set; }
        public string GameState { get; set; }
        public int NumberOfPlayers { get; set; }
    }
}
