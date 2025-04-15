namespace Wego.API.Models.DTOS
{
    public class AttractionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public int? Rating { get; set; }
        public bool IsFavorite { get; set; } = false;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Address { get; set; }
        public double? DistanceFromLocation { get; set; }
        public int? LocationId { get; set; }

    }
    public class AttractionPutDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int? Rating { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Address { get; set; }
        public double? DistanceFromLocation { get; set; }
        public int? LocationId { get; set; }

    }

    public class AttractionPostDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int? Rating { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Address { get; set; }
        public double? DistanceFromLocation { get; set; }
        public int? LocationId { get; set; }

    }
}
