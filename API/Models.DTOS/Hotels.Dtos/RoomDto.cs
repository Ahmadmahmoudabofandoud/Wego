using Wego.API.Models.DTOS.Hotels.Dtos;
using Wego.Core.Models.Enums;

namespace Wego.API.Models.DTOS.Rooms.Dtos
{
    public class RoomDto
    {
        public int Id { get; set; }
        public int? HotelId { get; set; }
        public string RoomDescribtion { get; set; }
        public string RoomTitle { get; set; }
        public string RoomLocation { get; set; }
        public int Rating { get; set; }
        public string RoomType { get; set; }
        public bool IsActive { get; set; }
        public List<AmenityDto> Amenities { get; set; } = new List<AmenityDto>();

        public List<string>? Images { get; set; }
        public List<RoomOptionDto> RoomOptions { get; set; } = new List<RoomOptionDto>();

    }

    public class RoomPostDto
    {
        public int? HotelId { get; set; }
        public required string RoomDescribtion { get; set; }
        public required string RoomTitle { get; set; }
        public required string RoomLocation { get; set; }
        public required int Rating { get; set; }
        public required RoomType RoomType { get; set; }
        public required bool IsActive { get; set; }
        public List<IFormFile>? Images { get; set; }
        public List<int> AmenityIds { get; set; } = new List<int>(); // IDs of amenities
        public List<RoomOptionDto> RoomOptions { get; set; } = new();

    }

    public class RoomPutDto
    {
        public int Id { get; set; }
        public int? HotelId { get; set; }
        public required string RoomDescribtion { get; set; }
        public required string RoomTitle { get; set; }
        public required string RoomLocation { get; set; }
        public required int Rating { get; set; }
        public required string RoomType { get; set; }
        public required bool IsActive { get; set; }
        public List<IFormFile>? NewImages { get; set; }
        public List<int> AmenityIds { get; set; } = new List<int>(); // IDs of amenities
        public List<int>? ImagesToDelete { get; set; }
        public List<RoomOptionCreateDto> RoomOptions { get; set; } = new List<RoomOptionCreateDto>(); // قائمة الخيارات

    }
}
