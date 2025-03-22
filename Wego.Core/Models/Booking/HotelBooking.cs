using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Enums;
using Wego.Core.Models.Identity;

namespace Wego.Core.Models.Booking
{
    public  class HotelBooking:BaseModel
    {

        public string? UserId { get; set; }
        public DateTime? BookingDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public BookingStatus? Status { get; set; }

        public virtual AppUser? User { get; set; }
        public virtual ICollection<RoomBooking> RoomBookings { get; set; } = new HashSet<RoomBooking>();
    }
}
