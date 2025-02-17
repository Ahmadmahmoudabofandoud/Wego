using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wego.Core.Models.Room
{
    public class RoomBookingDetails : BaseModel
    {
        [ForeignKey("Booking")]
        public Guid BookingId { get; set; }

        [ForeignKey("Room")]
        public Guid RoomId { get; set; }

        [Required]
        public DateOnly Checkin { get; set; }
        [Required]
        public DateOnly Checkout { get; set; }
        public int Guests { get; set; }
        public virtual Room Room { get; set; }
        public virtual Booking Booking { get; set; }

    }
}
