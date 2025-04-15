using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wego.Core.Models.Enums;

namespace Wego.Core.Models.Flights
{
    public class Airplane: BaseModel
    {

        public string? Code { get; set; }
        public string? Type { get; set; }
        public int? AirlineId { get; set; }
        public List<Features> Feature { get; set; } = new List<Features>(); 

        public virtual Airline? Airline { get; set; }
        public virtual ICollection<Flight> Flights { get; set; } = new HashSet<Flight>();

    }
}
