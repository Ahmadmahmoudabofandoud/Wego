using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using API.Errors;
using Wego.Core.Models.Identity;
using Wego.API.Models.DTOS.Identity;
using System.IO;
using System;
using Microsoft.AspNetCore.Http;
using Wego.API.Helpers;
using System.Security.Claims;
using Newtonsoft.Json;
using Wego.Core.Services.Contract;
using Microsoft.AspNetCore.Authorization;

namespace Wego.API.Controllers
{
    public class ProfileController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public ProfileController(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        [Authorize]
        [HttpPost("save-profile")]
        public async Task<ActionResult<ProfileBookingDto>> SaveProfile([FromBody] ProfilePostDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "User is not authenticated"));

            var currentUser = await _userManager.FindByIdAsync(userId);
            if (currentUser == null)
                return NotFound(new ApiResponse(404, "User not found"));

            if (!dto.IsGuest)
            {
                currentUser.DisplayName = dto.DisplayName;
                currentUser.PhoneNumber = dto.PhoneNumber;
                currentUser.PassportNumber = dto.PassportNumber;
                currentUser.Nationality = dto.Nationality;
                currentUser.Gender = dto.Gender;
                currentUser.NationalId = dto.NationalId;
                currentUser.TripPurpose = dto.TripPurpose;
                currentUser.SpecialNeeds = dto.SpecialNeeds;

                var updateResult = await _userManager.UpdateAsync(currentUser);
                if (!updateResult.Succeeded)
                    return BadRequest(new ApiResponse(400, "Failed to update user profile"));

                var updatedDto = _mapper.Map<ProfileBookingDto>(currentUser);
                return Ok(updatedDto);
            }

            var guest = new AppUser
            {
                DisplayName = dto.DisplayName,
                UserName = Guid.NewGuid().ToString().Substring(0, 10),
                Email = dto.Email ?? $"{Guid.NewGuid()}@guest.com",
                PhoneNumber = dto.PhoneNumber,
                PassportNumber = dto.PassportNumber,
                Nationality = dto.Nationality,
                Gender = dto.Gender,
                NationalId = dto.NationalId,
                TripPurpose = dto.TripPurpose,
                SpecialNeeds = dto.SpecialNeeds,
                IsGuest = true
            };

            var result = await _userManager.CreateAsync(guest);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Failed to add guest profile"));

            var guestDto = _mapper.Map<ProfileBookingDto>(guest);
            return Ok(guestDto);
        }



        [HttpGet("GetuserProfile")]
        public async Task<ActionResult<ProfileDto>> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ApiResponse(401, "User is not authenticated"));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound(new ApiResponse(404, "User not found"));

            var profileDto = _mapper.Map<ProfileDto>(user);
            profileDto.ProfileImageUrl = string.IsNullOrEmpty(user.ProfileImageUrl)
                ? null
                : $"{Request.Scheme}://{Request.Host.Value}{user.ProfileImageUrl}";

            return Ok(profileDto);
        }

        [HttpPut("UpdateUserProfile")]
        public async Task<ActionResult<ProfileDto>> UpdateProfile([FromForm] ProfileUpdateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ApiResponse(401, "User is not authenticated"));
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "User not found"));
            }

            _mapper.Map(dto, user);
            if (dto.ProfileImageUrl != null)
            {
                user.ProfileImageUrl = await ProcessProfileImageAsync(dto.ProfileImageUrl, "profileImg", user.ProfileImageUrl);
            }
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse(400, "Failed to update profile"));
            }

            return await GetProfile();
        }



        private async Task<string?> ProcessProfileImageAsync(IFormFile? imageFile, string folder, string? existingImage = null)
        {
            if (imageFile == null) return existingImage;

            if (!string.IsNullOrEmpty(existingImage))
            {
                var oldImageName = Path.GetFileName(existingImage);
                ImageHelper.RemoveImage(folder, oldImageName);
            }

            string newImageName = $"profile-{Guid.NewGuid()}.jpg";
            await ImageHelper.UploadImageAsync(imageFile, folder, newImageName);

            return $"/imgs/{folder}/{newImageName}";
        }
    }
}
