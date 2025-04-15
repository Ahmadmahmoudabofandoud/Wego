using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.API.Models.DTOS.Locations.Dtos;
using Wego.Core.Models;
using Wego.Core.Specifications.LocationSpacification;

namespace Wego.Core.Services.Contract
{

    public interface ILocationService
    {
        Task<IReadOnlyList<LocationWithHotelsResponseDto>> GetNearbyLocationsAsync(AppSpecParamsNearby specParams);
    }

}
