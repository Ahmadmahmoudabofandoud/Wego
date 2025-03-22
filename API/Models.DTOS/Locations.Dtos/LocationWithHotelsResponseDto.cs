using Wego.API.Models.DTOS.Hotels.Dtos;

namespace Wego.API.Models.DTOS.Locations.Dtos
{
    public class LocationWithHotelsResponseDto
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Image { get; set; }
        public List<HotelDto> Hotels { get; set; } = new();
    }
}
