using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Wego.Core.Models.Flights;

namespace Wego.Core.Models.Booking
{
    public class SeatReservation : BaseModel
    {
        public Guid? SeatId { get; set; }

        [ForeignKey("SeatId")]
        public virtual Seat? Seat { get; set; }

        [Required]
        public Guid FlightBookingId { get; set; }
        [ForeignKey("FlightBookingId")]
        public virtual FlightBooking FlightBooking { get; set; }
    }
}
