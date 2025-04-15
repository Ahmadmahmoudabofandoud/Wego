using System.ComponentModel.DataAnnotations;

namespace Wego.Core.Models.Flights
{
    public class Airline : BaseModel
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Image { get; set; }

        public int? Rating { get; set; }


        public virtual ICollection<Airplane> Airplanes { get; set; }= new HashSet<Airplane>();
        public virtual ICollection<Flight> Flights { get; set; } = new HashSet<Flight>();
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();

    }
}
