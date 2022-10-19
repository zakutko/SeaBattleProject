using DAL.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("Game")]
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Column("gameStateId")]
        public int GameStateId { get; set; }
        public virtual GameState GameState { get; set; }
        [NotMapped]
        public GameStateEnum GameStateEnum { get; set; }
    }
}
