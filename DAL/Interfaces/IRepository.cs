using DAL.Models;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(int? id);
        Task<T> GetById(string id);   
        Task<IEnumerable<T>> GetAll();
        void CreateAsync(T entity);
        void UpdateAsync(T entity);
        void Delete(T entity);
    }
}
