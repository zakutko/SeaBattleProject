using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class ShipWrapperService : IShipWrapperService
    {
        private readonly IRepository<ShipWrapper> _repository;
        private readonly IRepository<Ship> _shipRepository;

        public ShipWrapperService(IRepository<ShipWrapper> repository, IRepository<Ship> shipRepository)
        {
            _repository = repository;
            _shipRepository = shipRepository;
        }

        public ShipWrapper CreateShipWrapper(int shipId, int fieldId)
        {
            return new ShipWrapper { ShipId = shipId, FieldId = fieldId };
        }

        public ShipWrapper GetDefaultShipWrapper(int fieldId)
        {
            return new ShipWrapper { FieldId = fieldId };
        }

        public int GetNumberOfShips(int fieldId)
        {
            return _repository.GetAll().Result.Where(x => x.FieldId == fieldId && x.ShipId != null).Count();
        }

        public int GetNumberOfShipsWhereSizeOne(int fieldId)
        {
            var shipWrapperList = _repository.GetAll().Result.Where(x => x.FieldId == fieldId && x.ShipId != null).ToList();
            var shipList = new List<Ship>();

            foreach(var shipWrapper in shipWrapperList)
            {
                if (shipWrapper.ShipId == null)
                {
                    return 0;
                }

                var ship = _shipRepository.GetById(shipWrapper.ShipId).Result;

                if (ship.ShipSizeId == 1)
                {
                    shipList.Add(ship);
                }
            }

            return shipList.Count();
        }

        public int GetNumberOfShipsWhereSizeTwo(int fieldId)
        {
            var shipWrapperList = _repository.GetAll().Result.Where(x => x.FieldId == fieldId && x.ShipId != null).ToList();
            var shipList = new List<Ship>();

            foreach (var shipWrapper in shipWrapperList)
            {
                var ship = _shipRepository.GetById(shipWrapper.ShipId).Result;
                if (ship.ShipSizeId == 2)
                {
                    shipList.Add(ship);
                }
            }

            return shipList.Count();
        }

        public int GetNumberOfShipsWhereSizeThree(int fieldId)
        {
            var shipWrapperList = _repository.GetAll().Result.Where(x => x.FieldId == fieldId && x.ShipId != null).ToList();
            var shipList = new List<Ship>();

            foreach (var shipWrapper in shipWrapperList)
            {
                var ship = _shipRepository.GetById(shipWrapper.ShipId).Result;
                if (ship.ShipSizeId == 3)
                {
                    shipList.Add(ship);
                }
            }

            return shipList.Count();
        }

        public int GetNumberOfShipsWhereSizeFour(int fieldId)
        {
            var shipWrapperList = _repository.GetAll().Result.Where(x => x.FieldId == fieldId && x.ShipId != null).ToList();
            var shipList = new List<Ship>();

            foreach (var shipWrapper in shipWrapperList)
            {
                var ship = _shipRepository.GetById(shipWrapper.ShipId).Result;
                if (ship.ShipSizeId == 4)
                {
                    shipList.Add(ship);
                }
            }

            return shipList.Count();
        }

        public IEnumerable<ShipWrapper> GetAllShipWrappersByFiedlId(int fieldId)
        {
            return _repository.GetAll().Result.Where(x => x.FieldId == fieldId);
        }
    }
}
