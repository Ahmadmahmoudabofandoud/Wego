using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Enums;
using Wego.Core.Models.Identity;

namespace Wego.Core.Models.Hotels
{
    public partial class Hotel:BaseModel
    {

        public string? Name { get; set; }
        public string? AboutHotel { get; set; }

        public int? LocationId { get; set; }
        public int? Rating { get; set; }
        public List<string>? Policies { get; set; }

        public virtual ICollection<Amenity> Amenities { get; set; }= new HashSet<Amenity>();
        public virtual Location? Location { get; set; }
        public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();
        public virtual ICollection<Room> Rooms { get; set; } = new HashSet<Room>();
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();

    }
}
