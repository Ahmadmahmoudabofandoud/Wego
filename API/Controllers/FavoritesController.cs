using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Wego.Core.Services.Contract;
using Wego.Core.Models;
using Wego.API.Helpers;
using API.Errors;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wego.API.Models.DTOS;
using Wego.Core.Models.Flights;
using Wego.Core.Models.Hotels;
using Wego.Core.Models.Identity;
using Wego.Core;
using Microsoft.AspNetCore.Authorization;
using Wego.API.Models.DTOS.Hotels.Dtos;
using Wego.API.Models.DTOS.Locations.Dtos;
using Wego.Core.Specifications.HotelSpecification;
using Wego.Core.Specifications.LocationSpacification;

namespace Wego.API.Controllers
{
    public class FavoritesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public FavoritesController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("{type}/{id}")]
        public async Task<ActionResult> AddToFavorites(string type, int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized(new ApiResponse(401));

            if (!await EntityExists(type, id)) return NotFound(new ApiResponse(404));

            // Check if already favorited
            var existingFavorites = await _unitOfWork.Repository<Favorite>().GetAsync(f =>
                f.UserId == currentUser.Id &&
                ((type.ToLower() == "location" && f.LocationId == id) ||
                 (type.ToLower() == "hotel" && f.HotelId == id))
            );

            if (existingFavorites.Any())
                return BadRequest(new ApiResponse(400, $"{type} is already in favorites."));

            var favorite = new Favorite
            {
                UserId = currentUser.Id,
                LocationId = type.ToLower() == "location" ? id : (int?)null,
                HotelId = type.ToLower() == "hotel" ? id : (int?)null
            };

            await _unitOfWork.Repository<Favorite>().Add(favorite);
            await _unitOfWork.CompleteAsync();

            if (type.ToLower() == "location")
            {
                var spec = new LocationWithAirportsAndHotelsSpecification(id); 
                var location = await _unitOfWork.Repository<Location>().GetEntityWithSpecAsync(spec);
                var locationDto = _mapper.Map<LocationsDto>(location);
                locationDto.IsFavorite = true;
                return Ok(locationDto);
            }

            if (type.ToLower() == "hotel")
            {
                var spec = new HotelWithDetailsSpecification(id); // Ensure this spec includes related data
                var hotel = await _unitOfWork.Repository<Hotel>().GetEntityWithSpecAsync(spec);
                var hotelDto = _mapper.Map<HotelDto>(hotel);
                hotelDto.IsFavorite = true;
                return Ok(hotelDto);
            }

            return BadRequest(new ApiResponse(400, "Invalid type."));
        }




        [HttpDelete("{type}/{id}")]
        public async Task<ActionResult> RemoveFromFavorites(string type, int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized(new ApiResponse(401));

            var favorite = (await _unitOfWork.Repository<Favorite>().GetAsync(f =>
                f.UserId == currentUser.Id &&
                ((type.ToLower() == "location" && f.LocationId == id) ||
                 (type.ToLower() == "hotel" && f.HotelId == id)))).FirstOrDefault();

            if (favorite == null) return NotFound(new ApiResponse(404));

            _unitOfWork.Repository<Favorite>().Delete(favorite);
            await _unitOfWork.CompleteAsync();

            // Get and return the unfavorited entity as response
            if (type.ToLower() == "location")
            {
                var spec = new LocationWithAirportsAndHotelsSpecification(id);
                var location = await _unitOfWork.Repository<Location>().GetEntityWithSpecAsync(spec);
                var locationDto = _mapper.Map<LocationsDto>(location);
                locationDto.IsFavorite = false;
                return Ok(locationDto);
            }

            if (type.ToLower() == "hotel")
            {
                var spec = new HotelWithDetailsSpecification(id);
                var hotel = await _unitOfWork.Repository<Hotel>().GetEntityWithSpecAsync(spec);
                var hotelDto = _mapper.Map<HotelDto>(hotel);
                hotelDto.IsFavorite = false;
                return Ok(hotelDto);
            }

            return BadRequest(new ApiResponse(400, "Invalid type."));
        }


        [HttpGet("getFavorites")]
        public async Task<ActionResult<List<object>>> GetFavorites()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized(new ApiResponse(401));

            var favorites = await _unitOfWork.Repository<Favorite>().GetAsync(f => f.UserId == currentUser.Id);
            var results = new List<object>();

            foreach (var fav in favorites)
            {
                if (fav.LocationId.HasValue)
                {
                    var spec = new LocationWithAirportsAndHotelsSpecification(fav.LocationId.Value);
                    var location = await _unitOfWork.Repository<Location>().GetEntityWithSpecAsync(spec);
                    var dto = _mapper.Map<LocationsDto>(location);
                    dto.IsFavorite = true;
                    results.Add(dto);
                }
                else if (fav.HotelId.HasValue)
                {
                    var spec = new HotelWithDetailsSpecification(fav.HotelId.Value);
                    var hotel = await _unitOfWork.Repository<Hotel>().GetEntityWithSpecAsync(spec);
                    var dto = _mapper.Map<HotelDto>(hotel);
                    dto.IsFavorite = true;
                    results.Add(dto);
                }
            }

            return Ok(results);
        }


        private async Task<bool> EntityExists(string type, int id)
        {
            switch (type.ToLower())
            {
                case "location":
                    return await _unitOfWork.Repository<Location>().GetByIdAsync(id) != null;
                case "hotel":
                    return await _unitOfWork.Repository<Hotel>().GetByIdAsync(id) != null;
                default:
                    return false;
            }
        }
    }
}