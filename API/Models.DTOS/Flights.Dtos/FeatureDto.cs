namespace Wego.API.Models.DTOS.Flights.Dtos
{
    public class FeatureDto
    {
        public bool Wifi { get; set; }
        public bool Usb { get; set; }
        public bool Meal { get; set; }
        public bool Video { get; set; }
    }

    public class FlightDto
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }

}
