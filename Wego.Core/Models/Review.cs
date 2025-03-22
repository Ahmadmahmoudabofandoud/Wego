using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Flights;
using Wego.Core.Models.Hotels;
using Wego.Core.Models.Identity;

namespace Wego.Core.Models
{
    public class Review : BaseModel
    {
        public string? UserId { get; set; }
        public decimal? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime? ReviewDate { get; set; }

        public virtual AppUser? User { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual Airline? Airline { get; set; }
    }

}
