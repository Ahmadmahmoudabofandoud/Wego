using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Wego.Core.Models.Booking;


namespace Wego.Core.Models.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public virtual ICollection<FlightBooking> FlightBookings { get; set; } = new List<FlightBooking>();
    }
}
