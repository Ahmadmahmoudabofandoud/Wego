using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wego.Core.Models.Flights
{
    public class Airplane: BaseModel
    {
        public Airplane()
        {
            Features = new HashSet<Feature>();
            Flights = new HashSet<Flight>();
        }
        public string? Code { get; set; }
        public string? Type { get; set; }
        public int? AirlineId { get; set; }

        public virtual Airline? Airline { get; set; }
        public virtual ICollection<Feature> Features { get; set; }
        public virtual ICollection<Flight> Flights { get; set; }

    }
}
