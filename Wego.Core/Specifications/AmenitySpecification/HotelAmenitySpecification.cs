using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Hotels;

namespace Wego.Core.Specifications.AmenitySpecification
{
    public class HotelAmenitySpecification : BaseSpecifcation<HotelAmenity>
    {
        public HotelAmenitySpecification(int amenityId)
            : base(ha => ha.AmenityId == amenityId)
        {
            Includes.Add(ha => ha.Hotel);
        }
    }
}
