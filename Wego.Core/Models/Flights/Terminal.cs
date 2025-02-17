using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wego.Core.Models.Flights
{
    public class Terminal : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Guid AirportId { get; set; }
        [ForeignKey("AirportId")]
        public virtual Airport Airport { get; set; }

        [InverseProperty("DepartureTerminal")]
        public virtual ICollection<Flight> DepartureFlights { get; set; } = new List<Flight>();

        [InverseProperty("ArrivalTerminal")]
        public virtual ICollection<Flight> ArriveFlights { get; set; } = new List<Flight>();
    }
}
