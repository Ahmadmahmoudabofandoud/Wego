using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Wego.Core.Models;
using Wego.Core.Specifications;

public class SearchSpecification<T> : BaseSpecifcation<T> where T : BaseModel
{
    public SearchSpecification(string searchTerm, Expression<Func<T, string>> property1, string sortOrder = "asc")
        : base(BuildSearchExpression(searchTerm, property1))
    {
        ApplySorting(property1, sortOrder);
    }

    public SearchSpecification(string searchTerm, Expression<Func<T, string>> property1, Expression<Func<T, string>> property2, string sortOrder = "asc")
        : base(BuildSearchExpression(searchTerm, property1, property2))
    {
        ApplySorting(property1, sortOrder);
    }

    // ✅ دالة تقوم ببناء Expression بدون Invoke()
    private static Expression<Func<T, bool>> BuildSearchExpression(string searchTerm, Expression<Func<T, string>> property1)
    {
        if (string.IsNullOrEmpty(searchTerm)) return _ => true;

        return entity => EF.Functions.Like(property1.Compile().Invoke(entity), $"%{searchTerm}%");
    }

    private static Expression<Func<T, bool>> BuildSearchExpression(string searchTerm, Expression<Func<T, string>> property1, Expression<Func<T, string>> property2)
    {
        if (string.IsNullOrEmpty(searchTerm)) return _ => true;

        return entity =>
            EF.Functions.Like(EF.Property<string>(entity, GetPropertyName(property1)), $"%{searchTerm}%") ||
            EF.Functions.Like(EF.Property<string>(entity, GetPropertyName(property2)), $"%{searchTerm}%");
    }

    private void ApplySorting(Expression<Func<T, string>> property, string sortOrder)
    {
        Expression<Func<T, object>> orderByExpression = Expression.Lambda<Func<T, object>>(Expression.Convert(property.Body, typeof(object)), property.Parameters);

        if (!string.IsNullOrEmpty(sortOrder))
        {
            switch (sortOrder.ToLower())
            {
                case "asc":
                    AddOrderBy(orderByExpression);
                    break;
                case "desc":
                    AddOrderByDesc(orderByExpression);
                    break;
                default:
                    AddOrderBy(orderByExpression);
                    break;
            }
        }
    }

    // ✅ دالة لاستخراج اسم الحقل من Expression<Func<T, string>>
    private static string GetPropertyName(Expression<Func<T, string>> property)
    {
        if (property.Body is MemberExpression member)
            return member.Member.Name;

        throw new ArgumentException("Invalid property expression");
    }
}
