using DAL.Data;
using DAL.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class PositionRepository : Repository<Position>, IPositionRepository, IRepository<Position>
    {
        public PositionRepository(DataContext context) : base(context)
        {
        }

        public IEnumerable<Position> GetByShipWrapperId(IEnumerable<ShipWrapper> shipWrappers)
        {
            var positionList = new List<Position>();

            foreach (var shipWrapper in shipWrappers)
            {
                var positions = _context.Positions.Where(x => x.ShipWrapperId == shipWrapper.Id);

                foreach (var position in positions)
                {
                    positionList.Add(position);
                }
            }

            return positionList;
        }

        public IEnumerable<int> GetAllCellIds(IEnumerable<Position> positions)
        {
            var cellList = new List<int>();

            foreach (var position in positions)
            {
                cellList.Add(position.CellId);
            }

            return cellList;
        }
    }
}
