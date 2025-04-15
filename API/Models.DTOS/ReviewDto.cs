namespace Wego.API.Models.DTOS
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? UserFullName { get; set; }
        public decimal? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string? HotelName { get; set; }
        public string? AirlineName { get; set; }
    }

    public class ReviewPostDto
    {
        public decimal? Rating { get; set; }
        public string? Comment { get; set; }
        public int? HotelId { get; set; } = null;
        public int? AirlineId { get; set; } = null;
    }

    public class ReviewPutDto
    {
        public int Id { get; set; }
        public decimal? Rating { get; set; }
        public string? Comment { get; set; }
    }
}
