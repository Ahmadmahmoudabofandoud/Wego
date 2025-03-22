namespace Wego.API.Models.DTOS.Hotels.Dtos
{
    public class AmenityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class AmenityPostDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class HotelAmenityDto
    {
        public int HotelId { get; set; }
        public int AmenityId { get; set; }
        public string AmenityName { get; set; } = string.Empty;
    }

    public class HotelAmenityPostDto
    {
        public int HotelId { get; set; }
        public int AmenityId { get; set; }
    }
}
