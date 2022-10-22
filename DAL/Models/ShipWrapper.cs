using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("ShipWrapper")]
    public class ShipWrapper
    {
        [Key]
        public int Id { get; set; }
        [Column("shipId")]
        public int? ShipId { get; set; }
        [Column("fieldId")]
        public int FieldId { get; set; }
        public virtual Ship Ship { get; set; }
        public virtual Field Field { get; set; }
    }
}
