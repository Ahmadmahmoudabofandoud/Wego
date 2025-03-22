
using Wego.Core.Models.Flights;

namespace Wego.Core.Specifications.AirlineSpecification
{
    public class AirlineWithAirplanesAndFlightsSpecification : BaseSpecifcation<Airline>
    {
        public AirlineWithAirplanesAndFlightsSpecification(AppSpecParams specParams)
            : base(A =>
                    (string.IsNullOrEmpty(specParams.Search) ||
                     A.Name.ToLower().Contains(specParams.Search) ||
                     A.Code.ToLower().Contains(specParams.Search))
            )
        {
            AddIncludes();

            // ترتيب حسب الشهرة (عدد الطائرات + عدد الرحلات)
            AddOrderByDesc(A => (A.Airplanes.Count + A.Flights.Count));

            // دعم الترتيب حسب الاسم أو الكود
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

        public AirlineWithAirplanesAndFlightsSpecification(int id)
            : base(A => A.Id == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(A => A.Airplanes);
            Includes.Add(A => A.Flights);
            Includes.Add(A => A.Reviews);
        }
    }
}
