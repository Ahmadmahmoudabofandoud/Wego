using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wego.Core.Models.Hotels
{
    public class RoomOption : BaseModel
    {
        public required string Title { get; set; }             
        public required int Price { get; set; }                
        public bool IsRefundable { get; set; }                 
        public bool IncludesBreakfast { get; set; }            
        public int Capacity { get; set; }                     

        public int RoomId { get; set; }
        public virtual Room Room { get; set; } = null!;
    }

}
