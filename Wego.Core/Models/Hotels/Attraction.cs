using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wego.Core.Models.Hotels
{
    public class Attraction : BaseModel
    {
        public string Name { get; set; } 
        public string Description { get; set; } 
        public string Category { get; set; }
        public string Image { get; set; } 
        public int? Rating { get; set; } 
        public double? Latitude { get; set; } 
        public double? Longitude { get; set; } 
        public string Address { get; set; } 
        public double? DistanceFromLocation { get; set; } 

        public virtual Location? Location { get; set; }
    }

}
