using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("GameHistory")]
    public class GameHistory
    {
        [Key]
        public int Id { get; set; }
        [Column("gameId")]
        public int GameId { get; set; }
        [Column("firstPlayerName")]
        public string FirstPlayerName { get; set; }
        [Column("secondPlayerName")]
        public string SecondPlayerName { get; set; }
        [Column("gameStateName")]
        public string GameStateName { get; set; }
        [Column("winnerName")]
        public string WinnerName { get; set; }
    }
}
