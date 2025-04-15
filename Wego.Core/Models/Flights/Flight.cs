using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wego.Core.Models.Booking;
using Wego.Core.Models.Enums;

namespace Wego.Core.Models.Flights
{
    public class Flight : BaseModel
    {
        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public FlightStatus Status { get; set; } = FlightStatus.Scheduled;

        public double EconomyClassPrice { get; set; }

        public double BusinessClassPrice { get; set; }

        public double FirstClassPrice { get; set; }

        public int? AirlineId { get; set; }
        public int AirplaneId { get; set; }
        public int ArrivalAirportId { get; set; }  
        public int DepartureAirportId { get; set; }

        public virtual Airline? Airline { get; set; }
        public virtual Airplane Airplane { get; set; }
        public virtual Airport? ArrivalAirport { get; set; }
        public virtual Airport? DepartureAirport { get; set; }

        public virtual ICollection<FlightBooking> FlightBookings { get; set; } = new List<FlightBooking>();

    }
}
