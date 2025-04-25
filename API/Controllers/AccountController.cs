using API.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Wego.API.Models.DTOS.Identity;
using Wego.Core.Models.Identity;
using Wego.Core.Services.Contract;

namespace Wego.API.Controllers
{
    public class AccountController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, IAuthService authService, IEmailService emailService) : BaseApiController
    {
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly IAuthService _authService = authService;
        private readonly IEmailService _emailService= emailService;
        private readonly UserManager<AppUser> _userManager = userManager;


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiValidationErrorResponse() { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray() });

            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This email is already in use!!" } });

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


        // POST: /api/account/logout
        [Authorize] // تأكد من أن المستخدم مسجّل الدخول
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out successfully" });
        }

        // POST: /api/account/forgot-password
        [HttpPost("forgot-password")]
        public async Task<ActionResult<ApiResponse>> ForgotPassword(ForgotPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return Unauthorized(new ApiResponse(401, "User not found"));

            var code = new Random().Next(100000, 999999).ToString();

            await _userManager.SetAuthenticationTokenAsync(user, "Default", "ResetPassword", code);

            string projectName = "Stay Safe"; 
            string subject = $"{projectName} - Password Reset Request";
            string body = $@"
            <div style='font-family:Arial,sans-serif; max-width:600px; margin:auto; padding:20px; border:1px solid #eee; border-radius:10px;'>
                <h2 style='color:#4CAF50;'>{projectName} - Password Reset</h2>
    
                <p>Dear <strong>{user.DisplayName}</strong>,</p>

                <p>We received a request to reset your password for your {projectName} account.</p>

                <p>Your password reset code is:</p>
                <div style='font-size:24px; font-weight:bold; color:#333; background:#f5f5f5; padding:10px 15px; width:max-content; border-radius:5px;'>
                    {code}
                </div>

                <p>If you didn't request this, please ignore this email.</p>

                <br />
                <p style='color:gray; font-size:12px;'>{projectName} App Team</p>
            </div>";

            await _emailService.SendEmailAsync(user.Email, subject, body, isBodyHTML: true);

            return Ok(new ApiResponse(200, "If your email is registered, you will receive a password reset email shortly."));
        }

        // POST: /api/account/verify-code
        [HttpPost("verify-code")]
        public async Task<ActionResult<ApiResponse>> VerifyCode(VerifyCodeDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return Unauthorized(new ApiResponse(401, "User not found"));

            var storedCode = await _userManager.GetAuthenticationTokenAsync(user, "Default", "ResetPassword");

            if (string.IsNullOrEmpty(storedCode) || storedCode != model.Code)
            {
                return BadRequest(new ApiResponse(400, "Invalid verification code"));
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            return Ok(new
            {
                Message = "Verification code is valid.",
                ResetToken = resetToken 
            });
        }



        // POST: /api/account/reset-password
        [HttpPost("reset-password")]
        public async Task<ActionResult<UserDto>> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (model.NewPassword != model.ConfirmPassword)
                return BadRequest(new ApiResponse(400, "Passwords do not match"));

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return Unauthorized(new ApiResponse(401, "User not found"));

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Failed to reset password"));

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(new UserDto
            {
                Status = true,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }


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