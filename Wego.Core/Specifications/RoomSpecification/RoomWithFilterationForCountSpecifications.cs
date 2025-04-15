using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Hotels;

namespace Wego.Core.Specifications.RoomSpecification
{
    public class RoomWithFilterationForCountSpecifications : BaseSpecifcation<Room>
    {
        public RoomWithFilterationForCountSpecifications(RoomSpecParams specParams)
            : base(R =>
                    (string.IsNullOrEmpty(specParams.Search) ||
                     (R.RoomTitle != null && R.RoomTitle.ToLower().Contains(specParams.Search)) ||
                     (R.RoomLocation != null && R.RoomLocation.ToLower().Contains(specParams.Search)) ||
                     (R.RoomDescribtion != null && R.RoomDescribtion.ToLower().Contains(specParams.Search)) ||
                     (R.Hotel != null && R.Hotel.Name.ToLower().Contains(specParams.Search))) &&
                    (!specParams.Price.HasValue || R.RoomOptions.Any(ro => ro.Price == specParams.Price.Value)) &&
                    (!specParams.Rating.HasValue || R.Rating >= specParams.Rating.Value) && 
                    (!specParams.RoomType.HasValue || R.RoomType == specParams.RoomType.Value) && 
                    (!specParams.IsActive.HasValue || R.IsActive == specParams.IsActive.Value) 
        )
        {
        }
    }

}
