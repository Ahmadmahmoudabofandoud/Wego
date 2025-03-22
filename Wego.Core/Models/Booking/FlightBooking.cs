using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Identity;
using Wego.Core.Models.Enums;
using Wego.Core.Models.Flights;

namespace Wego.Core.Models.Booking
{
    public class FlightBooking : BaseModel
    {
        public string? UserId { get; set; }

        public int FlightId { get; set; }

        public double? TotalPrice { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public FlightBookingStatus Status { get; set; } = FlightBookingStatus.Pending;

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public virtual AppUser? User { get; set; }

        public virtual Flight Flight { get; set; }

        public virtual ICollection<SeatReservation> SeatReservations { get; set; } = new List<SeatReservation>();
    }
}
