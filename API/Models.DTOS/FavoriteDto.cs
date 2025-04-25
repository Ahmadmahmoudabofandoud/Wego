namespace Wego.API.Models.DTOS
{
    public class FavoriteDto
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int? HotelId { get; set; }
        public int? LocationId { get; set; }
        public int? AttractionId { get; set; }

    }
}
