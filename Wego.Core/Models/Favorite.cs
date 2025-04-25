using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Flights;
using Wego.Core.Models.Hotels;
using Wego.Core.Models.Identity;

namespace Wego.Core.Models
{
    public class Favorite : BaseModel
    {
        public string? UserId { get; set; } 
        public int? HotelId { get; set; } 
        public int? AirlineId { get; set; } 
        public int? LocationId { get; set; }
        public int? AttractionId { get; set; }

        public virtual AppUser User { get; set; }
        public virtual Hotel? Hotel { get; set; }
        public virtual Airline? Airline { get; set; }
        public virtual Location? Location { get; set; }
        public virtual Attraction? Attraction { get; set; }

    }

}
