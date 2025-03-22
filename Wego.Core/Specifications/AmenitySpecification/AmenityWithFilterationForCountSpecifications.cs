using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Hotels;

namespace Wego.Core.Specifications.AmenitySpecification
{
    public class AmenityWithFilterationForCountSpecifications : BaseSpecifcation<Amenity>
    {
        public AmenityWithFilterationForCountSpecifications(AppSpecParams specParams)
            : base(A =>
                    string.IsNullOrEmpty(specParams.Search) ||
                    A.Name.ToLower().Contains(specParams.Search)
            )
        {
        }
    }

}
