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
        public HotelWithFilterationForCountSpecifications(AppSpecParams specParams)
            : base(H =>
                    (string.IsNullOrEmpty(specParams.Search) ||
                     H.Name.ToLower().Contains(specParams.Search) ||
                     H.Location.City.ToLower().Contains(specParams.Search) ||
                     H.Location.Country.ToLower().Contains(specParams.Search) ||
                     H.Rooms.Any(r => r.RoomTitle.ToLower().Contains(specParams.Search)) ||
                     H.HotelAmenities.Any(ha => ha.Amenity.Name.ToLower().Contains(specParams.Search)) 
                    )
            )
        {
        }
    }

}
