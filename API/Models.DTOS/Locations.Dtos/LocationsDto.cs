using System.Text.Json.Serialization;

namespace Wego.API.Models.DTOS.Locations.Dtos
{
    public class LocationsDto
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Image { get; set; }
        public int? Rating { get; set; }
        public int? AverageRoomPrice { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool IsFavorite { get; set; } = false;

    }

    public class LocationPostDto
    {
        public string Country { get; set; }
        public string City { get; set; }
        public int? Rating { get; set; }
        public int? AverageRoomPrice { get; set; }
        public IFormFile? ImageFile { get; set; } 
    }

    public class LocationPutDto
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int? Rating { get; set; }
        public int? AverageRoomPrice { get; set; }
        public IFormFile? ImageFile { get; set; } 
    }


}
