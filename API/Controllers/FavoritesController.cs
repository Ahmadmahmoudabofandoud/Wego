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

            var favorite = new Favorite
            {
                UserId = currentUser.Id,
                LocationId = type.ToLower() == "location" ? id : (int?)null,
                HotelId = type.ToLower() == "hotel" ? id : (int?)null,
                AirlineId = type.ToLower() == "airline" ? id : (int?)null,
            };

            await _unitOfWork.Repository<Favorite>().Add(favorite);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = $"{type} added to favorites." });
        }

        [HttpDelete("{type}/{id}")]
        public async Task<ActionResult> RemoveFromFavorites(string type, int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized(new ApiResponse(401));

            var favorite = await _unitOfWork.Repository<Favorite>().GetAsync(f =>
                f.UserId == currentUser.Id &&
                ((type.ToLower() == "location" && f.LocationId == id) ||
                 (type.ToLower() == "hotel" && f.HotelId == id) ||
                 (type.ToLower() == "airline" && f.AirlineId == id))
            );

            if (!favorite.Any()) return NotFound(new ApiResponse(404));

            _unitOfWork.Repository<Favorite>().Delete(favorite.First());
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = $"{type} removed from favorites." });
        }

        [HttpGet]
        public async Task<ActionResult<List<FavoriteDto>>> GetFavorites()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized(new ApiResponse(401));

            var favorites = await _unitOfWork.Repository<Favorite>().GetAsync(f => f.UserId == currentUser.Id);
            var favoriteDtos = _mapper.Map<List<FavoriteDto>>(favorites);

            return Ok(favoriteDtos);
        }

        private async Task<bool> EntityExists(string type, int id)
        {
            switch (type.ToLower())
            {
                case "location":
                    return await _unitOfWork.Repository<Location>().GetByIdAsync(id) != null;
                case "hotel":
                    return await _unitOfWork.Repository<Hotel>().GetByIdAsync(id) != null;
                case "airline":
                    return await _unitOfWork.Repository<Airline>().GetByIdAsync(id) != null;
                default:
                    return false;
            }
        }
    }
}