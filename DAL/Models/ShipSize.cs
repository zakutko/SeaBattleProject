using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("ShipSize")]
    public class ShipSize
    {
        [Key, ForeignKey("Ship")]
        public int Id { get; set; }
        [Column("shipSizeName")]
        public string ShipSizeName { get; set; }
    }
}
