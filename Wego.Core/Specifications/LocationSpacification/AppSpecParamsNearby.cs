using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wego.Core.Specifications.LocationSpacification
{
    public class AppSpecParamsNearby : AppSpecParams
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double MaxDistance { get; set; } = 50;
    }
}
