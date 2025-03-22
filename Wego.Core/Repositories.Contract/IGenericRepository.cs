using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models;
using Wego.Core.Specifications;

namespace Wego.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : BaseModel
    {

        #region Static Queries
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        #endregion

        #region Dynamic Quries Using SpecificationDesignPattern
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);

        Task<T> GetEntityWithSpecAsync(ISpecification<T> spec);

        #endregion

        // Pagination Count
        Task<int> GetCountWithSpecAsync(ISpecification<T> spec);

        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }

}
