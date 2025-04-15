using System.Linq;
using System.Linq.Expressions;
using Wego.Core.Models;
using Wego.Core.Models.Hotels;

namespace Wego.Core.Specifications.HotelSpecification
{
    public class HotelWithDetailsSpecification : BaseSpecifcation<Hotel>
    {
        public HotelWithDetailsSpecification(HotelSpecParams specParams)
            : base(H =>
                     (string.IsNullOrEmpty(specParams.Search) ||
                      (H.Name != null && H.Name.ToLower().Contains(specParams.Search)) ||
                      (H.Location != null && H.Location.City.ToLower().Contains(specParams.Search)) ||
                      (H.Location != null && H.Location.Country.ToLower().Contains(specParams.Search))
                     ) &&
                     (!specParams.LocationId.HasValue || H.LocationId == specParams.LocationId) &&
                     (!specParams.MinRating.HasValue || H.Rating >= specParams.MinRating)
            )
        {
            AddIncludes();


            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "ratingDesc":
                        AddOrderByDesc(H => H.Rating);
                        break;
                    case "ratingAsc":
                        AddOrderBy(H => H.Rating);
                        break;
                    case "nameAsc":
                        AddOrderBy(H => H.Name);
                        break;
                    case "nameDesc":
                        AddOrderByDesc(H => H.Name);
                        break;
                }

            }

            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }

        public HotelWithDetailsSpecification(int id)
            : base(H => H.Id == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(h => h.Images);
            Includes.Add(h => h.Location);
            Includes.Add(h => h.Reviews);
            Includes.Add(h => h.Amenities);



            ThenIncludes.Add((
                (Expression<Func<Hotel, object>>)(h => h.Rooms),
                (Expression<Func<object, object>>)(r => ((Room)r).Images)));
            ThenIncludes.Add((
                (Expression<Func<Hotel, object>>)(h => h.Rooms),
                (Expression<Func<object, object>>)(r => ((Room)r).Amenities)));
            ThenIncludes.Add((
                (Expression<Func<Hotel, object>>)(h => h.Rooms),
                (Expression<Func<object, object>>)(r => ((Room)r).RoomOptions)));

        }
    }
}
