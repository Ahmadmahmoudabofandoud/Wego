using System.Text.Json.Serialization;
using Wego.Core.Models.Enums;
using Wego.Core.Models.Flights;

namespace Wego.API.Models.DTOS.Flights.Dtos
{
    public class FlightDTO
    {
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public FlightStatus Status { get; set; }
        public double EconomyClassPrice { get; set; }
        public double BusinessClassPrice { get; set; }
        public double FirstClassPrice { get; set; }
        public int? AirlineId { get; set; }
        public int AirplaneId { get; set; }
        public int DepartureAirportId { get; set; }
        public int ArrivalAirportId { get; set; }
        public int? NoOfBookings { get; set; }
    }

    public class FlightTicketDTO
    {
        public int Id { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string Status { get; set; }
        public double EconomyClassPrice { get; set; }
        public double BusinessClassPrice { get; set; }
        public double FirstClassPrice { get; set; }
        public string AirlineName { get; set; }
        public string AirplaneType { get; set; }
        public string ArrivalAirportName { get; set; }
        public string DepartureAirportName { get; set; }

        public string DurationMinutes { get; set; }
    }
}
