using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Enums;
using Wego.Core.Models.Flights;
using Wego.Core.Models.Hotels;
using Wego.Core.Models.Identity;

namespace Wego.Core.Models
{
    public class Image : BaseModel
    {
        public string Url { get; set; } = null!;
        public int? HotelId { get; set; }
        public int? RoomId { get; set; }
        public int? AirlineId { get; set; }

        public virtual Airline? Airline { get; set; }
        public virtual Hotel? Hotel { get; set; }
        public virtual Room? Room { get; set; }
    }
}
