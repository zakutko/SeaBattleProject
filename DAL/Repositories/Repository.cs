using DAL.Data;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DataContext _context;
        public Repository(DataContext context)
        {
            _context = context;
        }

        public T GetById(int? id)
        {
            return _context.Set<T>().Find(id);
        }

        public T GetById(string id)
        {
            return _context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void CreateAsync(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void UpdateAsync(T entity)
        {
            _context.Update(entity);
        }
    }
}
