using System.ComponentModel.DataAnnotations;

namespace Wego.API.Models.DTOS.Hotels.Dtos
{
    public class AmenityDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
    }
    public class AmenityPostDto
    {
        [Required]
        public string Name { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
    public class AmenityPutDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IFormFile? Image { get; set; }
    }

}
