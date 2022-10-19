using DAL.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("Ship")]
    public class Ship
    {
        [Key]
        public int Id { get; set; }
        [Column("directionId")]
        public int DirectionId { get; set; }
        [Column("shipStateId")]
        public int ShipStateId { get; set; }
        [Column("shipSizeId")]
        public int ShipSizeId { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual ShipState ShipState { get; set; }
        public virtual ShipSize ShipSize { get; set; }
        public virtual ShipWrapper ShipWrapper { get; set; }
        [NotMapped]
        public virtual DirectionEnum DirectionEnum { get; set; }
        [NotMapped]
        public virtual ShipStateEnum StateEnum { get; set; }
        [NotMapped]
        public virtual ShipSizeEnum SizeEnum { get; set; }
    }
}
