using System.ComponentModel.DataAnnotations;

namespace WEB.ViewModels
{
    public class CreateShipViewModel
    {
        [Required]
        public int ShipDirection { get; set; }
        [Required]
        public int ShipSize { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public int X { get; set; }
        [Required]
        public int Y { get; set; }
    }
}
