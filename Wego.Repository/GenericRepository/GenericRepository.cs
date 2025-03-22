using Castle.Components.DictionaryAdapter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models;
using Wego.Core.Repositories.Contract;
using Wego.Core.Specifications;
using Wego.Repository.Data;

namespace Wego.Repository.GenericRepository
{
    public class GenericRepository<T>(ApplicationDbContext dbContext) : IGenericRepository<T> where T : BaseModel
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        #region Static Queries
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
                return await _dbContext.Set<T>().ToListAsync();
        }


        public async Task<T> GetByIdAsync(int id)
        {
            //return await _dbContext.Set<T>().Where(X => X.Id == id).FirstOrDefault();
            return await _dbContext.Set<T>().FindAsync(id);
        }
        #endregion

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        public async Task Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        #region DemooOld


        //   public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>>? condition = null)
        //   {
        //       if (condition is { })
        //       {
        //           return await _dbContext.Set<T>().Where(condition).ToListAsync();
        //       }
        //       return await _dbContext.Set<T>().ToListAsync();
        //   }
        //   public async Task AddAsync(T entity)
        //      => await _dbContext.AddAsync(entity);
        //   public void Update(T entity)
        //       => _dbContext.Update(entity);
        //   public void Delete(T entity)
        //     => _dbContext.Remove(entity);


        //   private static IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortColumn, string sortDirection)
        //   {
        //       var parameter = Expression.Parameter(typeof(T), "x");
        //       var property = Expression.Property(parameter, sortColumn);
        //       var lambda = Expression.Lambda(property, parameter);

        //       string method = sortDirection.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";
        //       var methodCallExpression = Expression.Call(typeof(Queryable), method,
        //           new Type[] { typeof(T), property.Type }, query.Expression, Expression.Quote(lambda));

        //       return query.Provider.CreateQuery<T>(methodCallExpression);
        //   }


        //   public async Task<IEnumerable<T>> SearchAsync(
        //Expression<Func<T, bool>> predicate,
        //string sortColumn = "Id",
        //string sortDirection = "asc",
        //int pageNumber = 1,
        //int pageSize = 10,
        //params string[] includeProperties) // إزالة `?` لأن `params` بالفعل يمكن أن يكون فارغًا
        //   {
        //       IQueryable<T> query = _dbContext.Set<T>().Where(predicate);

        //       // ✅ تطبيق العلاقات المطلوبة فقط إذا تم تمريرها
        //       if (includeProperties.Length > 0)
        //       {
        //           foreach (var includeProperty in includeProperties)
        //           {
        //               query = query.Include(includeProperty);
        //           }
        //       }

        //       // ✅ التأكد من أن `sortColumn` صالح وإلا سيتم الترتيب افتراضيًا
        //       if (!string.IsNullOrWhiteSpace(sortColumn))
        //       {
        //           query = sortDirection.ToLower() == "desc"
        //               ? query.OrderByDescending(e => EF.Property<object>(e, sortColumn))
        //               : query.OrderBy(e => EF.Property<object>(e, sortColumn));
        //       }

        //       // ✅ تطبيق الترقيم (Pagination) وتحميل البيانات
        //       return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        //   }


        //   #region old
        //   //public async Task<IEnumerable<T>> SearchAsync(
        //   //    Expression<Func<T, bool>> predicate,
        //   //    string sortColumn = "Id",
        //   //    string sortDirection = "asc",
        //   //    int pageNumber = 1,
        //   //    int pageSize = 10,
        //   //    string? includeProperties = null
        //   //)
        //   //{
        //   //    IQueryable<T> query = _dbContext.Set<T>().AsQueryable(); 

        //   //    query = query.Where(predicate); // ✅ تطبيق الشرط

        //   //    if (!string.IsNullOrWhiteSpace(includeProperties))
        //   //    {
        //   //        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        //   //        {
        //   //            query = query.Include(includeProperty);
        //   //        }
        //   //    }

        //   //    query = sortDirection.ToLower() == "asc"
        //   //        ? query.OrderBy(e => EF.Property<object>(e, sortColumn))
        //   //        : query.OrderByDescending(e => EF.Property<object>(e, sortColumn));

        //   //    return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        //   //} 
        //   #endregion



        //   public async Task<T?> GetAsync(int id, params string[] includeProperties)
        //   {
        //       IQueryable<T> query = _dbContext.Set<T>();

        //       foreach (var includeProperty in includeProperties)
        //       {
        //           query = query.Include(includeProperty);
        //       }

        //       return await query.FirstOrDefaultAsync(e => e.Id == id);
        //   } 
        #endregion


    }

}
