using Wego.API.Models.DTOS.Rooms.Dtos;
using Wego.Core.Models.Hotels;

namespace Wego.API.Models.DTOS.Hotels.Dtos
{
    public class HotelDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? AboutHotel { get; set; }
        public int? LocationId { get; set; }
        public int? Rating { get; set; }
        public string? LocationName { get; set; }
        public bool IsFavorite { get; set; } = false;
        public List<string>? Images { get; set; } = new List<string>();
        public List<AmenityDto>? Amenities { get; set; } = new List<AmenityDto>();
        public List<RoomDto>? Rooms { get; set; } = new List<RoomDto>();
        public List<string>? Policies { get; set; }

        public List<int>? ReviewIds { get; set; } = new List<int>(); 
    }


    public class HotelPostDto
    {
        public string Name { get; set; } = string.Empty;
        public string? AboutHotel { get; set; }
        public int LocationId { get; set; }
        public decimal Rating { get; set; }
        public List<IFormFile>? Images { get; set; }
        public List<string>? Policies { get; set; }
        public List<int> AmenityIds { get; set; } = new List<int>();
    }
    public class HotelPutDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? AboutHotel { get; set; }
        public bool IsFavorite { get; set; } = false;

        public int LocationId { get; set; }
        public decimal Rating { get; set; }
        public List<string>? Policies { get; set; }
        public List<IFormFile>? NewImages { get; set; } 
        public List<int>? ImagesToDelete { get; set; }
        public List<int> AmenityIds { get; set; } = new List<int>();
    }
    public class HotelAvailabilityQueryDto
    {
        public int HotelId { get; set; }
        public DateTime Checkin { get; set; } = DateTime.Now;
        public DateTime Checkout { get; set; } = DateTime.Now;
        public int Guests { get; set; } = 1;
        public int Children { get; set; } = 1;

        public string? Sort { get; set; }

        // pagination (defaults if not provided)
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
