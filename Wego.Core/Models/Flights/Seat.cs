using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Wego.Core.Models.Enums;
using Wego.Core.Models.Booking;

namespace Wego.Core.Models.Flights
{
    public class Seat : BaseModel
    {
        [Required]
        public string Number { get; set; }

        [Required]
        public SeatClass Class { get; set; } = SeatClass.Economy;

        [Required]
        public bool IsAvailable { get; set; } = true;

        [Required]
        public Guid AirplaneId { get; set; }

        [ForeignKey("AirplaneId")]
        public virtual Airplane Airplane { get; set; }

        public virtual ICollection<SeatReservation> SeatReservations { get; set; } = new List<SeatReservation>();

    }
}
