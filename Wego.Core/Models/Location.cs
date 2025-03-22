using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Wego.Core.Models.Flights;
using Wego.Core.Models.Hotels;

namespace Wego.Core.Models
{
    public class Location : BaseModel
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Image { get; set; }

        public virtual ICollection<Airport> Airports { get; set; } = new HashSet<Airport>();
        public virtual ICollection<Hotel> Hotels { get; set; } = new HashSet<Hotel>();
    }
}
