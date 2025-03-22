

namespace Wego.Core.Models.Hotels
{
    public  class RoomAmenity
    {
        public int? RoomId { get; set; }
        public int? AmenityId { get; set; }

        public virtual Amenity? Amenity { get; set; }
        public virtual Room? Room { get; set; }
    }
}
