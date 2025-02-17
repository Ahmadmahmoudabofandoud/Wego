using API.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;
using Wego.API.Models.DTOS.Identity;
using Wego.Core.Models.Identity;
using Wego.Core.Services.Contract;

namespace Wego.API.Controllers
{
    public class AccountController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, IAuthService authService) : BaseApiController
    {
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly IAuthService _authService = authService;
        private readonly UserManager<AppUser> _userManager = userManager;




        [HttpPost("register")] // POST : /api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "this email is already in user!!" } });

            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return Ok(new UserDto()
            {
                Status = true,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });

        }

        [HttpPost("login")] // POST : /api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
                return Unauthorized(new ApiResponse(401));

            var checkPass = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!checkPass.Succeeded)
                return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                Status = true,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }

        [HttpGet("emailexists")] // GET : /api/account/emailexists?email
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
             => await _userManager.FindByEmailAsync(email) is not null;




        [HttpDelete("delete/{email}")] // DELETE: /api/account/delete/{email}
        public async Task<ActionResult<UserDto>> DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return NotFound(new ApiResponse(404, "User not found"));

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Failed to delete user"));

            return Ok(new { message = "User deleted successfully" });
        }

    }
}