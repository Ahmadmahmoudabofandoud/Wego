using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Wego.Core.Models.Enums;
using Wego.Core.Models.Identity;


namespace Wego.Core.Models.Room
{
    public class Booking : BaseModel
    {

        [ForeignKey("User")]
        public string UserId { get; set; }

        public required DateTime BookingDate { get; set; } = DateTime.Now;

        public required int TotalPrice { get; set; }

        public required BookingStatus Status { get; set; }

        public virtual AppUser User { get; set; }

        public virtual IList<RoomBookingDetails> RoomBookingDetails { get; set; } = new List<RoomBookingDetails>();
    }

}
