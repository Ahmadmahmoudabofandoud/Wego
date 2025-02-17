using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Wego.Core.Models.Flights
{
    public class Feature : BaseModel
    {
        public bool Meal { get; set; } = false;
        public bool Wifi { get; set; } = false;
        public bool Video { get; set; } = false;
        public bool Usb { get; set; } = false;

        [JsonIgnore]
        public Guid AirplaneId { get; set; }
        [ForeignKey("AirplaneId")]
        [JsonIgnore]
        public virtual Airplane Airplane { get; set; }
    }
}
