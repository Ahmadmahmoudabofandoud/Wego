using System.Text.Json.Serialization;

namespace Wego.API.Models.DTOS.Locations.Dtos
{
    public class LocationsDto
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Image { get; set; } // رابط الصورة
    }

    public class LocationPostDto
    {
        public string Country { get; set; }
        public string City { get; set; }
        public IFormFile? ImageFile { get; set; } // الصورة المرفوعة
    }

    public class LocationPutDto
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public IFormFile? ImageFile { get; set; } // الصورة الجديدة المرفوعة
    }


}
