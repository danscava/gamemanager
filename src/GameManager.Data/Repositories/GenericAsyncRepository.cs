using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using GameManager.Data.Interfaces;
using GameManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GameManager.Data.Repositories
{
    /// <summary>
    /// Generic repository with most common operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericAsyncRepository<T> : IAsyncRepository<T> where T : Audit
    {
        protected readonly GameManagerContext _dbContext;

        public GenericAsyncRepository(GameManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> AddOrUpdateAsync(T entity)
        {
            var existing = await GetByIdAsync(entity.Id);

            if (existing == null)
                return await AddAsync(entity);
            return await UpdateAsync(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            entity.Active = 0; 
            await UpdateAsync(entity);
        }

        public async Task<int> SaveChanges()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
