using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("PlayerGame")]
    public class PlayerGame
    {
        [Key]
        public int Id { get; set; }
        [Column("gameId")]
        public int GameId { get; set; }
        [Column("firstPlayerId")]
        public string? FirstPlayerId { get; set; }
        [Column("secondPlayerId")]
        public string? SecondPlayerId { get; set; }
        public virtual AppUser FirstPlayer { get; set; }
        public virtual AppUser SecondPlayer { get; set; }
        public virtual Game Game { get; set; }
    }
}
