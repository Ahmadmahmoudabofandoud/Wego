using Microsoft.AspNetCore.Identity;
using Wego.Core.Models.Booking;


namespace Wego.Core.Models.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PassportNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public virtual ICollection<HotelBooking> HotelBookings { get; set; } = new HashSet<HotelBooking>();
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<FlightBooking> FlightBookings { get; set; } = new HashSet<FlightBooking>();
    }
}
