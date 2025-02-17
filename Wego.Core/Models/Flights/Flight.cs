using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wego.Core.Models.Booking;
using Wego.Core.Models.Enums;

namespace Wego.Core.Models.Flights
{
    public class Flight : BaseModel
    {
        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        public FlightStatus Status { get; set; } = FlightStatus.Scheduled;

        [Required]
        public double EconomyClassPrice { get; set; }

        [Required]
        public double BusinessClassPrice { get; set; }

        [Required]
        public double FirstClassPrice { get; set; }

        public Guid? AirlineId { get; set; }
        [ForeignKey("AirlineId")]
        public virtual Airline? Airline { get; set; }

        public Guid AirplaneId { get; set; }
        [ForeignKey("AirplaneId")]
        public virtual Airplane Airplane { get; set; }

        [Required]
        public Guid DepartureTerminalId { get; set; }
        [ForeignKey("DepartureTerminalId")]
        public virtual Terminal DepartureTerminal { get; set; }

        [Required]
        public Guid ArrivalTerminalId { get; set; }
        [ForeignKey("ArrivalTerminalId")]
        public virtual Terminal ArrivalTerminal { get; set; }

        public virtual ICollection<FlightBooking> FlightBookings { get; set; } = new List<FlightBooking>();

    }
}
