﻿namespace WEB.ViewModels
{
    public class GameHistoryViewModel
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string FirstPlayerName { get; set; }
        public string SecondPlayerName { get; set; }
        public string GameStateName { get; set; }
        public string WinnerName { get; set; }
    }
}
