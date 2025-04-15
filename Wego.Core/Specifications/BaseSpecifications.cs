using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models;

namespace Wego.Core.Specifications
{
    public class BaseSpecifcation<T> : ISpecification<T> where T : BaseModel
    {
        public Expression<Func<T, bool>> Criteria { get; set; } 
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public List<(Expression<Func<T, object>>, Expression<Func<object, object>>)> ThenIncludes { get; set; } = new();

        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Take { get; set; }
        public int SKip { get; set; }
        public bool IsPaginationEnabled { get; set; }


        public BaseSpecifcation()
        {
            // Criteria = null;
        }

        public BaseSpecifcation(Expression<Func<T, bool>> inputExpression)
        {
            Criteria = inputExpression; // P => P.BrandId == 2 && true
        }

        public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression; // P => P.Name
        }

        public void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDesc = orderByDescExpression;
        }
        protected void IncludeWith(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        public void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabled = true;

            SKip = skip;
            Take = take;

        }

    }
}
