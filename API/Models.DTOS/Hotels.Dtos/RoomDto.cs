namespace Wego.API.Models.DTOS.Rooms.Dtos
{
    public class RoomDto
    {
        public int Id { get; set; }
        public int? HotelId { get; set; }
        public string RoomDescribtion { get; set; }
        public string RoomTitle { get; set; }
        public string RoomLocation { get; set; }
        public int Price { get; set; }
        public int Rating { get; set; }
        public string RoomType { get; set; }
        public bool IsActive { get; set; }
        public List<string>? Images { get; set; }
    }

    public class RoomPostDto
    {
        public int? HotelId { get; set; }
        public required string RoomDescribtion { get; set; }
        public required string RoomTitle { get; set; }
        public required string RoomLocation { get; set; }
        public required int Price { get; set; }
        public required int Rating { get; set; }
        public required string RoomType { get; set; }
        public required bool IsActive { get; set; }
        public List<IFormFile>? Images { get; set; }
    }

    public class RoomPutDto
    {
        public int Id { get; set; }
        public int? HotelId { get; set; }
        public required string RoomDescribtion { get; set; }
        public required string RoomTitle { get; set; }
        public required string RoomLocation { get; set; }
        public required int Price { get; set; }
        public required int Rating { get; set; }
        public required string RoomType { get; set; }
        public required bool IsActive { get; set; }
        public List<IFormFile>? NewImages { get; set; }
        public List<int>? ImagesToDelete { get; set; }
    }
}
