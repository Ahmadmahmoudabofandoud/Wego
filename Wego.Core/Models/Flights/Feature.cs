using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wego.Core.Models.Flights
{
    public class Feature :BaseModel
    {

        public bool? Meal { get; set; }
        public bool? Wifi { get; set; }
        public bool? Video { get; set; }
        public bool? Usb { get; set; }
        public int? AirplaneId { get; set; }

        public virtual Airplane? Airplane { get; set; }

    }
}
