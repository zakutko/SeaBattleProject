using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("GameState")]
    public class GameState
    {
        [Key, ForeignKey("Game")]
        public int Id { get; set; }
        [Column("gameStateName")]
        public string GameStateName { get; set; }
    }
}
