using System.ComponentModel.DataAnnotations;
using System.Xml;
using Wego.Core.Models.Enums;
using Wego.Core.Models.Flights;


namespace Wego.Core.Models.Room
{
    public class Room : BaseModel
    {
        public required string RoomDescribtion { get; set; }

        public required string RoomTitle { get; set; }
        public required string RoomAddress { get; set; }

        public required string RoomOwner { get; set; }
        //public required string RoomOwnerId { get; set; }

        public required string City { get; set; }

        public required string Country { get; set; }

        public required int Price { get; set; }
        public required int Rating { get; set; }

        public required RoomType RoomType { get; set; }

        public required bool IsActive { get; set; } //status

        public virtual ICollection<Images> Images { get; set; } = new List<Images>();
        public virtual ICollection<Feature> Amentities { get; set; } = new List<Feature>();


        public virtual ICollection<RoomBookingDetails> RoomBookingDetails { get; set; } = new List<RoomBookingDetails>();

    }

}
