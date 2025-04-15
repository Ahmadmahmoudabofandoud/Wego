using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Hotels;

namespace Wego.Core.Specifications.RoomSpecification
{
    public class RoomWithDetailsSpecification : BaseSpecifcation<Room>
    {
        public RoomWithDetailsSpecification(RoomSpecParams specParams)
            : base(R =>
                    (string.IsNullOrEmpty(specParams.Search) ||
                     (R.RoomTitle != null && R.RoomTitle.ToLower().Contains(specParams.Search)) ||
                     (R.RoomLocation != null && R.RoomLocation.ToLower().Contains(specParams.Search)) ||
                     (R.RoomDescribtion != null && R.RoomDescribtion.ToLower().Contains(specParams.Search))) &&
                    (!specParams.Price.HasValue || R.RoomOptions.Any(ro => ro.Price == specParams.Price.Value)) && 
                    (!specParams.Rating.HasValue || R.Rating >= specParams.Rating.Value) &&
                    (!specParams.RoomType.HasValue || R.RoomType == specParams.RoomType.Value) && 
                    (!specParams.IsActive.HasValue || R.IsActive == specParams.IsActive.Value) 
        )
        {
            AddIncludes();

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceDesc":
                        AddOrderByDesc(R => R.RoomOptions.Max(ro => ro.Price));
                        break;
                    case "priceAsc":
                        AddOrderBy(R => R.RoomOptions.Min(ro => ro.Price));
                        break;
                    case "ratingDesc":
                        AddOrderByDesc(R => R.Rating);
                        break;
                    case "ratingAsc":
                        AddOrderBy(R => R.Rating);
                        break;
                    case "titleAsc":
                        AddOrderBy(R => R.RoomTitle);
                        break;
                    case "titleDesc":
                        AddOrderByDesc(R => R.RoomTitle);
                        break;
                }
            }

            // Apply pagination
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }

        public RoomWithDetailsSpecification(int id)
            : base(R => R.Id == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(R => R.Images);
            Includes.Add(R => R.Hotel);
            Includes.Add(R => R.RoomBookings);
            Includes.Add(R => R.Amenities);
            Includes.Add(R => R.RoomOptions); // إضافة RoomOptions هنا
        }
    }
}
