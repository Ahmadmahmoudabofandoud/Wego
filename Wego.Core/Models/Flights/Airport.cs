using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wego.Core.Models.Flights
{
    public class Airport : BaseModel
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? LocationId { get; set; }

        public virtual Location? Location { get; set; }
        public virtual ICollection<Flight> FlightArrivalAirports { get; set; } = new HashSet<Flight>();
        public virtual ICollection<Flight> FlightDepartureAirports { get; set; } = new HashSet<Flight>();
    }
}
