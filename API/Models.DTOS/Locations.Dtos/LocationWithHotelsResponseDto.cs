using Wego.API.Models.DTOS.Flights.Dtos;
using Wego.API.Models.DTOS.Hotels.Dtos;

namespace Wego.API.Models.DTOS.Locations.Dtos
{
    public class LocationWithHotelsResponseDto
    {
        public int? Id { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Image { get; set; }
        public int? Rating { get; set; }
        public int? AverageRoomPrice { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool IsFavorite { get; set; } = false;
        public List<HotelDto>? Hotels { get; set; } = new();
        //public List<AirportDto>? Airports { get; set; } = new();
    }
}
