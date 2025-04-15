using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Booking;
using Wego.Core.Models.Enums;

namespace Wego.Core.Models.Hotels
{
    public class Room : BaseModel
    {
        public int? HotelId { get; set; }

        public required string RoomDescribtion { get; set; }

        public required string RoomTitle { get; set; }
        public required string RoomLocation { get; set; }
        public required int Rating { get; set; }

        public required RoomType RoomType { get; set; }

        public required bool IsActive { get; set; } 
        public virtual Hotel? Hotel { get; set; }
        public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();
        public virtual ICollection<Amenity> Amenities { get; set; }= new HashSet<Amenity>();
        public virtual ICollection<RoomBooking> RoomBookings { get; set; } = new HashSet<RoomBooking>();
        public virtual ICollection<RoomOption> RoomOptions { get; set; } = new HashSet<RoomOption>();

    }
}
