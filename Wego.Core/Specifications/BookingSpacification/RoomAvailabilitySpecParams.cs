using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wego.Core.Specifications.BookingSpacification
{
    public class RoomAvailabilitySpecParams
    {
        public int HotelId { get; set; }
        public DateTime Checkin { get; set; } = DateTime.Now;
        public DateTime Checkout { get; set; } = DateTime.Now;
        public int Guests { get; set; } = 1;
        public int Children { get; set; } = 1;

        public string? Sort { get; set; }

        // pagination (defaults if not provided)
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
