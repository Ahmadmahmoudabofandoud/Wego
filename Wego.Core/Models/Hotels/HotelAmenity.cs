using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wego.Core.Models.Hotels
{
    public class HotelAmenity : BaseModel
    {
        public int HotelId { get; set; } 
        public int AmenityId { get; set; } 

        public virtual Amenity Amenity { get; set; } = null!;
        public virtual Hotel Hotel { get; set; } = null!;
    }
}
