using Microsoft.AspNetCore.Identity;
using Wego.Core.Models.Booking;
using Wego.Core.Models.Enums;


namespace Wego.Core.Models.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string? PassportNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Nationality? Nationality { get; set; }
        public Gender? Gender { get; set; }
        public string? Address { get; set; }
        public string? ProfileImageUrl { get; set; }

        public virtual ICollection<HotelBooking> HotelBookings { get; set; } = new HashSet<HotelBooking>();
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<FlightBooking> FlightBookings { get; set; } = new HashSet<FlightBooking>();

    }
}
