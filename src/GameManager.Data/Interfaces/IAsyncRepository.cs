using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.Specification;
using GameManager.Data.Models;

namespace GameManager.Data.Interfaces
{
    /// <summary>
    /// Generic async repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAsyncRepository<T> where T : Audit
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> AddOrUpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> SaveChanges();
    }
}
