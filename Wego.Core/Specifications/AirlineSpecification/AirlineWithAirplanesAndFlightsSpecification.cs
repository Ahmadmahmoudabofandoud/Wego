using Wego.Core.Models.Flights;
using Microsoft.EntityFrameworkCore;

namespace Wego.Core.Specifications.AirlineSpecification
{
    public class AirlineWithAirplanesAndFlightsSpecification : BaseSpecifcation<Airline>
        {
            public AirlineWithAirplanesAndFlightsSpecification(AirlineSpecParams specParams)
                : base(A =>
                         (string.IsNullOrEmpty(specParams.Search) ||
                          A.Name.ToLower().Contains(specParams.Search) ||
                          A.Code.ToLower().Contains(specParams.Search)) &&
                         (!specParams.MinRating.HasValue || A.Rating >= specParams.MinRating) &&
                         (!specParams.DepartureAirportId.HasValue ||
                          A.Flights.Any(f => f.DepartureAirportId == specParams.DepartureAirportId)) &&
                         (!specParams.ArrivalAirportId.HasValue ||
                          A.Flights.Any(f => f.ArrivalAirportId == specParams.ArrivalAirportId)) &&
                         (!specParams.DepartureDate.HasValue ||
                          A.Flights.Any(f => f.DepartureTime.Date == specParams.DepartureDate.Value.Date)) &&
                         (!specParams.MinPrice.HasValue ||
                          A.Flights.Any(f => f.EconomyClassPrice >= specParams.MinPrice)) &&
                         (!specParams.MaxPrice.HasValue ||
                          A.Flights.Any(f => f.EconomyClassPrice <= specParams.MaxPrice))
                )
            {
                AddIncludes();

                // ترتيب حسب الشهرة (عدد الطائرات + عدد الرحلات)
                AddOrderByDesc(A => (A.Airplanes.Count + A.Flights.Count));

                // دعم الترتيب حسب الاسم أو الكود أو التقييم
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
                        case "ratingAsc":
                            AddOrderBy(A => A.Rating);
                            break;
                        case "ratingDesc":
                            AddOrderByDesc(A => A.Rating);
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

