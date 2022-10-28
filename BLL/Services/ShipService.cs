using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class ShipService : IShipService
    {
        private readonly IRepository<Ship> _repository;

        public ShipService(IRepository<Ship> repository)
        {
            _repository = repository;
        }

        public Ship CreateShip(int directionId, int shipStateId, int shipSizeId)
        {
            return new Ship { DirectionId = directionId, ShipStateId = shipStateId, ShipSizeId = shipSizeId };
        }

        public IEnumerable<Ship> GetAllShips(IEnumerable<int> cellIds)
        {
            var ships = new List<Ship>();
            foreach (var cellId in cellIds)
            {
                ships.Add(_repository.GetById(cellId).Result);
            }
            return ships;
        }
    }
}
