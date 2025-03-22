using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wego.Core.Models.Hotels
{
    public class Amenity : BaseModel
    {
        public string? Name { get; set; }
        public virtual ICollection<HotelAmenity> HotelAmenities { get; set; } = new HashSet<HotelAmenity>();
    }
}
