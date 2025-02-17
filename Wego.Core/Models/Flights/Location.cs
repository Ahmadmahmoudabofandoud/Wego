using System.ComponentModel.DataAnnotations;

namespace Wego.Core.Models.Flights
{
    public class Location : BaseModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Country { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string City { get; set; }

        public string? Image { get; set; }
        public virtual ICollection<Airport> Airports { get; set; } = new List<Airport>();
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
