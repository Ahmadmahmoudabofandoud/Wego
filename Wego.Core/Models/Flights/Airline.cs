using System.ComponentModel.DataAnnotations;

namespace Wego.Core.Models.Flights
{
    public class Airline : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }
        public string? Image { get; set; }

        public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();

        public virtual ICollection<Airplane> Airplanes { get; set; } = new List<Airplane>();
    }
}
