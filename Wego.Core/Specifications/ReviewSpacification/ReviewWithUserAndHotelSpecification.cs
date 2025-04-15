using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models;
using Wego.Core.Models.Hotels;

namespace Wego.Core.Specifications.ReviewSpacification
{
    public class ReviewWithUserAndHotelSpecification : BaseSpecifcation<Review>
    {
        public ReviewWithUserAndHotelSpecification(AppSpecParams specParams)
            : base(R =>
                    (string.IsNullOrEmpty(specParams.Search) ||
                     R.User.DisplayName.ToLower().Contains(specParams.Search) ||
                     R.Hotel.Name.ToLower().Contains(specParams.Search))
            )
        {
            AddIncludes();

            AddOrderByDesc(R => R.ReviewDate);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "ratingAsc":
                        AddOrderBy(R => R.Rating);
                        break;
                    case "ratingDesc":
                        AddOrderByDesc(R => R.Rating);
                        break;
                    case "dateAsc":
                        AddOrderBy(R => R.ReviewDate);
                        break;
                    case "dateDesc":
                        AddOrderByDesc(R => R.ReviewDate);
                        break;
                }
            }

            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }

        public ReviewWithUserAndHotelSpecification(int id)
            : base(R => R.Id == id)
        {
            AddIncludes();
        }


        private void AddIncludes()
        {
            Includes.Add(R => R.User);
            Includes.Add(R => R.Hotel);
            Includes.Add(R => R.Airline);
        }
    }
    public class ReviewByHotelIdSpecification : BaseSpecifcation<Review>
    {
        public ReviewByHotelIdSpecification(AppSpecParams specParams)
            : base(R =>
                    (string.IsNullOrEmpty(specParams.Search) ||
                     R.User.DisplayName.ToLower().Contains(specParams.Search) ||
                     R.Hotel.Name.ToLower().Contains(specParams.Search))
            )
        {
            AddIncludes();

            AddOrderByDesc(R => R.ReviewDate);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "ratingAsc":
                        AddOrderBy(R => R.Rating);
                        break;
                    case "ratingDesc":
                        AddOrderByDesc(R => R.Rating);
                        break;
                    case "dateAsc":
                        AddOrderBy(R => R.ReviewDate);
                        break;
                    case "dateDesc":
                        AddOrderByDesc(R => R.ReviewDate);
                        break;
                }
            }

            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }

        public ReviewByHotelIdSpecification(int hotelId)
            : base(R => R.HotelId == hotelId)
        {
            AddIncludes();
        }


        private void AddIncludes()
        {
            Includes.Add(R => R.User);
            Includes.Add(R => R.Hotel);
            Includes.Add(R => R.Airline);
        }
    }

}
