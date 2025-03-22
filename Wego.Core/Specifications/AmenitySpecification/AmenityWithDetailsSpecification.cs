using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Hotels;

namespace Wego.Core.Specifications.AmenitySpecification
{
    public class AmenityWithDetailsSpecification : BaseSpecifcation<Amenity>
    {
        public AmenityWithDetailsSpecification(AppSpecParams specParams)
            : base(A =>
                    string.IsNullOrEmpty(specParams.Search) ||
                    A.Name.ToLower().Contains(specParams.Search)
            )
        {
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "nameAsc":
                        AddOrderBy(A => A.Name);
                        break;
                    case "nameDesc":
                        AddOrderByDesc(A => A.Name);
                        break;
                }
            }

            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }

        public AmenityWithDetailsSpecification(int id)
            : base(A => A.Id == id)
        {
        }
    }

}
