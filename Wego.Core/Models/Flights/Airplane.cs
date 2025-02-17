using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wego.Core.Models.Flights
{
    public class Airplane: BaseModel
    {

        [Required]
        public string Code { get; set; }

        [Required]
        public string Type { get; set; }

        public Guid? AirlineId { get; set; }
        [ForeignKey("AirlineId")]
        public virtual Airline? Airline { get; set; }

        public virtual Feature Feature { get; set; }
        public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();

        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
