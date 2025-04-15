using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Wego.Core.Models.Flights;

namespace Wego.Core.Models.Enums
{
    public enum Features
    {
        Meal,         
        Wifi,         
        Video,        
        USB,          
        PowerOutlet,  
        RecliningSeat,
        Entertainment,
        AirConditioning, 
        SleepingKit,  

    }
}
