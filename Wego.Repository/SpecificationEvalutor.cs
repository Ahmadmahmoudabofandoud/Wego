using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models;
using Wego.Core.Specifications;

namespace Wego.Repository
{
    internal static class SpecificationEvaluator<TEntity> where TEntity : BaseModel
    {

        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery; // _dbContext.Set<Order>()

            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            //  _dbContext.Set<Order>().Where(O => O.buyerEmail == Abdelramanzaky@gmail.com)
            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            //query = query.GroupBy(spec.OrderBy) ;

            //  _dbContext.Set<Product>().Where( P => P.BrandId == 2 && true).OrderBy(P => P.Name)

            else if (spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);

            if (spec.IsPaginationEnabled)
                query = query.Skip(spec.SKip).Take(spec.Take);



            query = spec.Includes.Aggregate(query, (currentQuery, queryExpression) => currentQuery.Include(queryExpression));
            // Apply ThenIncludes
            foreach (var (include, thenInclude) in spec.ThenIncludes)
            {
                query = query.Include(include).ThenInclude(thenInclude);
            }
            return query;
        }

    }


}
