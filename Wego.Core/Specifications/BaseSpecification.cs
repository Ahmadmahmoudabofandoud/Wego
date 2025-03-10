﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models;

namespace Wego.Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseModel
    {
        // This Class Create for Creating Object of Criteria and Includes
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDecsending { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }

        // CTOR of Includes [NO WHERE CONDITION] 
        // Calling Get All Product
        public BaseSpecification()
        {

        }

        // CTOR of WHERE | INCLUDES 
        // Calling Get Product By Id

        public BaseSpecification(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
        }

        public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        public void AddOrderByDecsending(Expression<Func<T, object>> orderByDecsendingExpression)
        {
            OrderByDecsending = orderByDecsendingExpression;
        }
        public void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }

}
