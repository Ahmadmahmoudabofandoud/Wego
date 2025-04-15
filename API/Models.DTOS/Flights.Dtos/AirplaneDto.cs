using Stripe.Entitlements;
using Wego.Core.Models.Enums;

namespace Wego.API.Models.DTOS.Flights.Dtos
{
    public class AirplaneDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Type { get; set; }
        public AirlineDto? Airline { get; set; }
        public List<string>? Features { get; set; } = new List<string>();
        public ICollection<FlightDto> Flights { get; set; } = new List<FlightDto>();
    }

    public class AirplanePostDto
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public int AirlineId { get; set; }
        public List<string>? Features { get; set; } = new List<string>();
    }

    public class AirplanePutDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public int AirlineId { get; set; }
        public List<string>? Features { get; set; } = new List<string>();
    }
}
