using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using API.Errors;
using Wego.Core.Models.Identity;
using Wego.API.Models.DTOS.Identity;

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

        [HttpGet("{userId}")]
        public async Task<ActionResult<ProfileDto>> GetProfile(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound(new ApiResponse(404, "User not found"));

            return Ok(_mapper.Map<ProfileDto>(user));
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult> UpdateProfile(string userId, [FromBody] ProfileUpdateDto dto)
        {
            if (userId != dto.Id) return BadRequest("User ID mismatch");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound(new ApiResponse(404, "User not found"));

            _mapper.Map(dto, user);
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400, "Failed to update profile"));

            return Ok(new ApiResponse(200, "Profile updated successfully"));
        }
    }
}
