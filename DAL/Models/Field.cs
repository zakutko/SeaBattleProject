using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("Field")]
    public class Field
    {
        [Key]
        public int Id { get; set; }
        [Column("size")]
        public int Size { get; set; }
        [Column("appUserId")]
        public string PlayerId { get; set; }
        public virtual AppUser Player { get; set; }
        public virtual ShipWrapper ?ShipWrapper { get; set; }
    }
}