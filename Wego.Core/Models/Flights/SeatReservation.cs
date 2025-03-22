using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Wego.Core.Models.Booking;

namespace Wego.Core.Models.Flights
{
    public class SeatReservation : BaseModel
    {
        public int? SeatId { get; set; }
        public int? FlightBookingId { get; set; }

        public virtual FlightBooking? FlightBooking { get; set; }
    }
}
