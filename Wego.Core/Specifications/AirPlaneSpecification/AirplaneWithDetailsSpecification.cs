using System.Linq;
using Wego.Core.Models;
using Wego.Core.Models.Flights;
using Wego.Core.Specifications;

namespace Wego.Core.Specifications.AirplaneSpecification
{
    public class AirplaneWithDetailsSpecification : BaseSpecifcation<Airplane>
    {
        public AirplaneWithDetailsSpecification(AppSpecParams specParams)
            : base(a =>
                    (string.IsNullOrEmpty(specParams.Search) ||
                     (a.Code != null && a.Code.ToLower().Contains(specParams.Search)) ||
                     (a.Type != null && a.Type.ToLower().Contains(specParams.Search)) ||
                     (a.Airline != null && a.Airline.Name.ToLower().Contains(specParams.Search))
                    )
            )
        {
            AddIncludes();

            // ترتيب حسب اسم الطائرة أو الكود
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "codeAsc":
                        AddOrderBy(a => a.Code);
                        break;
                    case "codeDesc":
                        AddOrderByDesc(a => a.Code);
                        break;
                    case "typeAsc":
                        AddOrderBy(a => a.Type);
                        break;
                    case "typeDesc":
                        AddOrderByDesc(a => a.Type);
                        break;
                }
            }

            // تطبيق Pagination
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }

        public AirplaneWithDetailsSpecification(int id)
            : base(a => a.Id == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(a => a.Airline);
            Includes.Add(a => a.Flights);
        }
    }
}
