using System.ComponentModel.DataAnnotations;
using Wego.API.Models.DTOS.Locations.Dtos;

namespace Wego.API.Models.DTOS.Flights.Dtos
{
    public class AirportDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? LocationId { get; set; }
        public LocationsDto? Location { get; set; }
    }

    public class AirportPostDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int LocationId { get; set; }
    }

    public class AirportPutDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? LocationId { get; set; }
    }

}
