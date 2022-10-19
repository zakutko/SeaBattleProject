using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("ShipState")]
    public class ShipState
    {
        [Key, ForeignKey("Ship")]
        public int Id { get; set; }
        [Column("shipStateName")]
        public string ShipStateName { get; set; }
    }
}
