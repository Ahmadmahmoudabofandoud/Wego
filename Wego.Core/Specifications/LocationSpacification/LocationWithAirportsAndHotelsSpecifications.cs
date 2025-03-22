using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models;

namespace Wego.Core.Specifications.LocationSpacification
{
    public class LocationWithAirportsAndHotelsSpecification : BaseSpecifcation<Location>
    {
        public LocationWithAirportsAndHotelsSpecification(AppSpecParams specParams)
            : base(L =>
                    (string.IsNullOrEmpty(specParams.Search) ||
                     L.City.ToLower().Contains(specParams.Search) ||
                     L.Country.ToLower().Contains(specParams.Search))
            )
        {
            AddIncludes();

            // ترتيب حسب الأكثر شهرة (عدد الفنادق + عدد المطارات)
            AddOrderByDesc(L => (L.Hotels.Count + L.Airports.Count));

            // دعم الترتيب حسب اسم المدينة أو الدولة
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "cityAsc":
                        AddOrderBy(L => L.City);
                        break;
                    case "cityDesc":
                        AddOrderByDesc(L => L.City);
                        break;
                    case "countryAsc":
                        AddOrderBy(L => L.Country);
                        break;
                    case "countryDesc":
                        AddOrderByDesc(L => L.Country);
                        break;
                }
            }

            // تطبيق Pagination
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }

        public LocationWithAirportsAndHotelsSpecification(int id)
            : base(P => P.Id == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(P => P.Hotels);
            Includes.Add(P => P.Airports);
        }
    }
}
