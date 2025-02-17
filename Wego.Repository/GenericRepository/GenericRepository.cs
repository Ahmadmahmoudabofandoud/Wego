using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models;
using Wego.Core.Repositories.Contract;
using Wego.Repository.Data;

namespace Wego.Repository.GenericRepository
{
    public class GenericRepository<T>(ApplicationDbContext dbContext) : IGenericRepository<T> where T : BaseModel
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<T?> GetAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public async Task AddAsync(T entity)
           => await _dbContext.AddAsync(entity);
        public void Update(T entity)
          => _dbContext.Set<T>().Update(entity);
        public void Delete(T entity)
          => _dbContext.Set<T>().Remove(entity);
    }
}
