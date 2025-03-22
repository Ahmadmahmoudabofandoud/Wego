using Wego.Core.Models;
using Wego.Core.Specifications;

public class LocationWithFilterationForCountSpecifications : BaseSpecifcation<Location>
{
    public LocationWithFilterationForCountSpecifications(AppSpecParams specParams)
        : base(L =>
                (string.IsNullOrEmpty(specParams.Search) ||
                 L.City.ToLower().Contains(specParams.Search) ||
                 L.Country.ToLower().Contains(specParams.Search) ||
                 L.Hotels.Any(h => h.Name.ToLower().Contains(specParams.Search)) ||
                 L.Airports.Any(a => a.Name.ToLower().Contains(specParams.Search))
                )
        )
    {
    }
}
