using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Flights;

namespace Wego.Core.Specifications.AirlineSpecification
{
    public class FlightsWithDetailsSpecification : BaseSpecifcation<Flight>
    {
        public FlightsWithDetailsSpecification(FlightSpecParams specParams)
            : base(F =>
                    (string.IsNullOrEmpty(specParams.Search) ||
                     F.Airline != null && F.Airline.Name.ToLower().Contains(specParams.Search) ||
                     F.DepartureAirport != null && F.DepartureAirport.Name.ToLower().Contains(specParams.Search) ||
                     F.ArrivalAirport != null && F.ArrivalAirport.Name.ToLower().Contains(specParams.Search)
                    ) &&
                    (!specParams.AirlineId.HasValue || F.AirlineId == specParams.AirlineId) &&
                    (!specParams.AirplaneId.HasValue || F.AirplaneId == specParams.AirplaneId) &&
                    (!specParams.DepartureAirportId.HasValue || F.DepartureAirportId == specParams.DepartureAirportId) &&
                    (!specParams.ArrivalAirportId.HasValue || F.ArrivalAirportId == specParams.ArrivalAirportId) &&
                    (!specParams.DepartureDate.HasValue || F.DepartureTime.Date == specParams.DepartureDate.Value.Date) &&
                    (!specParams.MinPrice.HasValue || F.EconomyClassPrice >= specParams.MinPrice) &&
                    (!specParams.MaxPrice.HasValue || F.EconomyClassPrice <= specParams.MaxPrice) &&
                    (!specParams.Status.HasValue || F.Status == specParams.Status)
            )
        {
            AddIncludes();

            // ترتيب حسب السعر أو موعد المغادرة
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(F => F.EconomyClassPrice);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(F => F.EconomyClassPrice);
                        break;
                    case "departureAsc":
                        AddOrderBy(F => F.DepartureTime);
                        break;
                    case "departureDesc":
                        AddOrderByDesc(F => F.DepartureTime);
                        break;
                }
            }

            // تطبيق Pagination
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }

        public FlightsWithDetailsSpecification(int id)
            : base(F => F.Id == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(f => f.Airline);
            Includes.Add(f => f.Airplane);
            Includes.Add(f => f.ArrivalAirport);
            Includes.Add(f => f.DepartureAirport);
            Includes.Add(f => f.FlightBookings);
        }
    }

}
