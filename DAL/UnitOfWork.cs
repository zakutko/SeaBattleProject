using DAL.Data;
using DAL.Interfaces;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;
        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Commit()
        {
            _dataContext.SaveChanges();
        }
        public void Dispose()
        {
            _dataContext.Dispose();
        }
    }
}
