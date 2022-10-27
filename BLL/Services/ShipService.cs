using BLL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class ShipService : IShipService
    {
        public Ship CreateShip(int directionId, int shipStateId, int shipSizeId)
        {
            return new Ship { DirectionId = directionId, ShipStateId = shipStateId, ShipSizeId = shipSizeId };
        }
    }
}
