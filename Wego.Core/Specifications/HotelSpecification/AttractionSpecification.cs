using Wego.Core.Specifications;
using Wego.Core.Models;
using Wego.Core.Models.Hotels;

public class AttractionSpecification : BaseSpecifcation<Attraction>
{
    public AttractionSpecification(AppSpecParams specParams)
        : base(A =>
                (string.IsNullOrEmpty(specParams.Search) ||
                 A.Name.ToLower().Contains(specParams.Search) ||
                 A.Description.ToLower().Contains(specParams.Search))
        )
    {
        AddIncludes();

        if (!string.IsNullOrEmpty(specParams.Sort))
        {
            switch (specParams.Sort)
            {
                case "nameAsc":
                    AddOrderBy(A => A.Name);
                    break;
                case "nameDesc":
                    AddOrderByDesc(A => A.Name);
                    break;
                case "ratingAsc":
                    AddOrderBy(A => A.Rating);
                    break;
                case "ratingDesc":
                    AddOrderByDesc(A => A.Rating);
                    break;
            }
        }

        ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
    }

    public AttractionSpecification(int id)
        : base(A => A.Id == id)
    {
        AddIncludes();
    }

    private void AddIncludes()
    {
        Includes.Add(A => A.Location);
    }
}