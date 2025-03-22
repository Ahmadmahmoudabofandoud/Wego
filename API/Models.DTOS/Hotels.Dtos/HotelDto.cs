using Wego.API.Models.DTOS.Rooms.Dtos;

namespace Wego.API.Models.DTOS.Hotels.Dtos
{
    public class HotelDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? LocationId { get; set; }
        public decimal? Rating { get; set; }
        public string? LocationName { get; set; }
        public List<string>? Images { get; set; } = new List<string>();
        public List<RoomDto> Rooms { get; set; } = new(); 

    }

    public class HotelPostDto
    {
        public string Name { get; set; } = string.Empty;
        public int LocationId { get; set; }
        public decimal Rating { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
    public class HotelPutDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int LocationId { get; set; }
        public decimal Rating { get; set; }
        public List<IFormFile>? NewImages { get; set; }  // صور جديدة يتم رفعها
        public List<int>? ImagesToDelete { get; set; }   // قائمة بمعرفات الصور التي سيتم حذفها
    }


}
