using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Hotels;

namespace Wego.Core.Models.Booking
{
    public class RoomBooking: BaseModel
    {
        [Required]
        public DateTime Checkin { get; set; }
        [Required]
        public DateTime Checkout { get; set; }
        public int Guests { get; set; }
        public int Children { get; set; }
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public int? RoomOptionId { get; set; }
        public virtual RoomOption? RoomOption { get; set; }
        public virtual Room? Room { get; set; }
        public virtual HotelBooking? Booking { get; set; }
    }
}
