using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Wego.Core.Models.Flights;
using Wego.Core.Models.Hotels;
using Wego.Core.Models.Identity;

namespace Wego.Core.Models
{
    public class Location : BaseModel
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Image { get; set; }
        public int? Rating { get; set; }
        public int? AverageRoomPrice { get; set; }

        public double? Latitude { get; set; }  
        public double? Longitude { get; set; }
        public virtual ICollection<AppUser> UsersWhoFavorite { get; set; } = new HashSet<AppUser>();

        public virtual ICollection<Airport> Airports { get; set; } = new HashSet<Airport>();
        public virtual ICollection<Hotel> Hotels { get; set; } = new HashSet<Hotel>();
    }
}
