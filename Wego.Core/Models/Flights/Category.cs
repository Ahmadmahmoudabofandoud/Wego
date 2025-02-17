using System.ComponentModel.DataAnnotations;

namespace Wego.Core.Models.Flights
{
    public class Category : BaseModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
    }
}
