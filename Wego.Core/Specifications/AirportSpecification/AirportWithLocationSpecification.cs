using System.Linq;
using Wego.Core.Models;
using Wego.Core.Models.Flights;
using Wego.Core.Specifications;

namespace Wego.Core.Specifications.AirportSpecification
{
    public class AirportWithLocationSpecification : BaseSpecifcation<Airport>
    {
        public AirportWithLocationSpecification(AppSpecParams specParams)
            : base(A =>
                    (string.IsNullOrEmpty(specParams.Search) ||
                     (A.Name != null && A.Name.ToLower().Contains(specParams.Search)) ||
                     (A.Code != null && A.Code.ToLower().Contains(specParams.Search)) ||
                     (A.Location != null && A.Location.City.ToLower().Contains(specParams.Search)) ||
                     (A.Location != null && A.Location.Country.ToLower().Contains(specParams.Search))
                    )
            )
        {
            AddIncludes();

            // ترتيب حسب الاسم أو الكود
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
                    case "codeAsc":
                        AddOrderBy(A => A.Code);
                        break;
                    case "codeDesc":
                        AddOrderByDesc(A => A.Code);
                        break;
                }
            }

            // تطبيق Pagination
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }

        public AirportWithLocationSpecification(int id)
            : base(A => A.Id == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(A => A.Location);
        }
    }
}
