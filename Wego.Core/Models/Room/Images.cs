using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Wego.Core.Models.Room
{
    public class Images : BaseModel
    {
        public required string URL { get; set; }

        [ForeignKey("Room")]
        public Guid RoomId { get; set; }
        public virtual Room Room { get; set; }
    }
}
