using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Hotels;

namespace Wego.Core.Specifications.HotelSpecification
{
    public class HotelWithFilterationForCountSpecifications : BaseSpecifcation<Hotel>
    {
        public HotelWithFilterationForCountSpecifications(HotelSpecParams specParams)
            : base(H =>
                    (string.IsNullOrEmpty(specParams.Search) ||
                     (H.Name != null && H.Name.ToLower().Contains(specParams.Search)) ||
                     (H.Location != null && H.Location.City.ToLower().Contains(specParams.Search)) ||
                     (H.Location != null && H.Location.Country.ToLower().Contains(specParams.Search)) ||
                     H.Rooms.Any(r => r.RoomTitle.ToLower().Contains(specParams.Search))
                    ) &&
                    (!specParams.LocationId.HasValue || H.LocationId == specParams.LocationId) &&
                    (!specParams.MinRating.HasValue || H.Rating >= specParams.MinRating)
            )
        {
        }
    }


}
