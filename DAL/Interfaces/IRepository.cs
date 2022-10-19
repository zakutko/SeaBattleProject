using DAL.Models;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        T GetById(string id);   
        IEnumerable<T> GetAll();
        void CreateAsync(T entity);
        void UpdateAsync(T entity);
        void Delete(T entity);
    }
}
