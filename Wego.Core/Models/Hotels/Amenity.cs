using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wego.Core.Models.Hotels
{
    public class Amenity:BaseModel
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public ICollection<Hotel> AmenityHotels { get; set; } = new HashSet<Hotel>();
        public ICollection<Room> AmenityRooms { get; set; } = new HashSet<Room>();
    }
}
