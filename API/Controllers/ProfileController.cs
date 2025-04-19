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

        [HttpPost("SaveGuestDetails")]
        public async Task<ActionResult<ProfileDto>> SaveGuestDetails([FromBody] ProfilePostDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "User is not authenticated"));

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new ApiResponse(404, "User not found"));

            _mapper.Map(dto, user);

            var result = await _userManager.UpdateAsync(user);
            return !result.Succeeded ? (ActionResult<ProfileDto>)BadRequest(new ApiResponse(400, "Failed to save guest details")) : await GetProfile();
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
