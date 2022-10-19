using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("CellState")]
    public class CellState
    {
        [Key, ForeignKey("Cell")]
        public int Id { get; set; }
        [Column("cellStateName")]
        public string CellStateName { get; set; }
    }
}