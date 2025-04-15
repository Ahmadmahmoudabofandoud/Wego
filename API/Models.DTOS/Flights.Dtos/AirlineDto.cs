using System.ComponentModel.DataAnnotations;

namespace Wego.API.Models.DTOS.Flights.Dtos
{
    public class AirlineDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public int Flights { get; set; }
        public int Airplanes { get; set; }
        public bool IsFavorite { get; set; } = false;

        public List<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();

    }
    public class AirlinePostDto
    {
        public string Name { get; set; } 

        public string Code { get; set; } 

        public IFormFile? ImageFile { get; set; }
    }
    public class AirlinePutDto
    {
        [Required]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public IFormFile? Image { get; set; }
    }


}
