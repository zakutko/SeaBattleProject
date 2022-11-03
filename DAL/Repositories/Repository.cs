using DAL.Data;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DataContext _context;
        public Repository(DataContext context)
        {
            _context = context;
        }

        public async Task<T> GetById(int? id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetById(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
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
