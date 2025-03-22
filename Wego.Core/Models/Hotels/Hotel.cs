using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wego.Core.Models.Hotels
{
    public partial class Hotel:BaseModel
    {

        public string? Name { get; set; }
        public int? LocationId { get; set; }
        public decimal? Rating { get; set; }

        public virtual Location? Location { get; set; }
        public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();
        public virtual ICollection<Room> Rooms { get; set; } = new HashSet<Room>();
        public virtual ICollection<HotelAmenity> HotelAmenities { get; set; } = new HashSet<HotelAmenity>();

    }
}
