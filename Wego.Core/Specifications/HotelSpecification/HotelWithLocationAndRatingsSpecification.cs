using System.Linq;
using Wego.Core.Models;
using Wego.Core.Models.Hotels;
using Wego.Core.Specifications;

namespace Wego.Core.Specifications.HotelSpecification
{
    public class HotelWithDetailsSpecification : BaseSpecifcation<Hotel>
    {
        public HotelWithDetailsSpecification(AppSpecParams specParams)
            : base(H =>
                    (string.IsNullOrEmpty(specParams.Search) ||
                     (H.Name != null && H.Name.ToLower().Contains(specParams.Search)) ||
                     (H.Location != null && H.Location.City.ToLower().Contains(specParams.Search)) ||
                     (H.Location != null && H.Location.Country.ToLower().Contains(specParams.Search)) 
                    )
            )
        {
            AddIncludes();

            // ترتيب حسب التقييم أو الاسم
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "ratingDesc":
                        AddOrderByDesc(H => H.Rating);
                        break;
                    case "ratingAsc":
                        AddOrderBy(H => H.Rating);
                        break;
                    case "nameAsc":
                        AddOrderBy(H => H.Name);
                        break;
                    case "nameDesc":
                        AddOrderByDesc(H => H.Name);
                        break;
                }
            }

            // تطبيق Pagination
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }

        public HotelWithDetailsSpecification(int id)
            : base(H => H.Id == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(H => H.Images);
            Includes.Add(H => H.Rooms);
            Includes.Add(H => H.Location);
        }





    }
}
