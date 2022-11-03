using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("Direction")]
    public class Direction
    {
        [Key, ForeignKey("Ship")]
        public int Id { get; set; }
        [Column("directionName")]
        public string DirectionName { get; set; }
    }
}
