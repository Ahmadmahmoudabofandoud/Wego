using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Wego.Core.Services.Contract;
using Wego.API.Models.DTOS.Locations.Dtos;
using Wego.Core.Specifications;
using Wego.Core;
using Wego.API.Helpers;
using API.Errors;
using Wego.Core.Specifications.LocationSpacification;
using Wego.Core.Models;
using Wego.Service;
using Microsoft.AspNetCore.Identity;
using Wego.Core.Models.Identity;

namespace Wego.API.Controllers
{

    public class LocationsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public LocationsController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<LocationsDto>>> GetAllLocations([FromQuery] AppSpecParams specParams)
        {
            var spec = new LocationWithAirportsAndHotelsSpecification(specParams);
            var locations = await _unitOfWork.Repository<Location>().GetAllWithSpecAsync(spec);
            var totalCount = await _unitOfWork.Repository<Location>().GetCountWithSpecAsync(spec);

            var currentUser = await _userManager.GetUserAsync(User);
            var favoriteLocationIds = currentUser != null
                ? (await _unitOfWork.Repository<Favorite>().GetUserIdAsync(currentUser.Id))
                    .Select(f => f.LocationId)
                    .ToList()
                : new List<int?>();

            var data = _mapper.Map<IReadOnlyList<Location>, IReadOnlyList<LocationsDto>>(locations);
            data = data.Select(loc =>
            {
                loc.IsFavorite = favoriteLocationIds.Contains(loc.Id);
                loc.Image = string.IsNullOrEmpty(loc.Image) ? null : $"{Request.Scheme}://{Request.Host.Value}{loc.Image}";
                return loc;
            }).ToList();

            return Ok(new Pagination<LocationsDto>(specParams.PageIndex, specParams.PageSize, totalCount, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LocationsDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<LocationsDto>> GetLocationById(int id)
        {
            var spec = new LocationWithAirportsAndHotelsSpecification(id);
            var location = await _unitOfWork.Repository<Location>().GetEntityWithSpecAsync(spec);
            if (location == null) return NotFound(new ApiResponse(404));

            var currentUser = await _userManager.GetUserAsync(User);
            var isFavorite = currentUser != null
                ? (await _unitOfWork.Repository<Favorite>().GetUserIdAsync(currentUser.Id))
                    .Any(f => f.LocationId == id)
                : false;

            var res = _mapper.Map<LocationsDto>(location);
            res.IsFavorite = isFavorite;
            res.Image = string.IsNullOrEmpty(res.Image) ? null : $"{Request.Scheme}://{Request.Host.Value}{res.Image}";

            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<LocationsDto>> AddLocation([FromForm] LocationPostDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var location = _mapper.Map<Location>(dto);
            location.Image = await ProcessLocationImageAsync(dto.ImageFile, "locationsImg");
            await _unitOfWork.Repository<Location>().Add(location);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetLocationById), new { id = location.Id }, _mapper.Map<LocationsDto>(location));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<LocationsDto>> UpdateLocation(int id, [FromForm] LocationPutDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.Id) return BadRequest("ID mismatch");

            var location = await _unitOfWork.Repository<Location>().GetByIdAsync(dto.Id);
            if (location == null) return NotFound(new ApiResponse(404));

            _mapper.Map(dto, location);
            location.Image = await ProcessLocationImageAsync(dto.ImageFile, "locationsImg", location.Image);

            _unitOfWork.Repository<Location>().Update(location);
            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<LocationsDto>(location));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLocation(int id)
        {
            var location = await _unitOfWork.Repository<Location>().GetByIdAsync(id);
            if (location == null) return NotFound(new ApiResponse(404));

            _unitOfWork.Repository<Location>().Delete(location);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = $"Location '{location.City}, {location.Country}' has been deleted successfully." });
        }

        private async Task<string?> ProcessLocationImageAsync(IFormFile? imageFile, string folder, string? existingImage = null)
        {
            if (imageFile == null) return existingImage;

            if (!string.IsNullOrEmpty(existingImage))
            {
                var oldImageName = Path.GetFileName(existingImage);
                ImageHelper.RemoveImage(folder, oldImageName);
            }

            string newImageName = $"location-{Guid.NewGuid()}.jpg";
            await ImageHelper.UploadImageAsync(imageFile, folder, newImageName);

            return $"/imgs/{folder}/{newImageName}";
        }
    }
}
