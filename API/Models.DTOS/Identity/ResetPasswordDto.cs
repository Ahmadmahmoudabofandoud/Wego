using System.ComponentModel.DataAnnotations;

namespace Wego.API.Models.DTOS.Identity
{
    public class ResetPasswordDto
    {
        [Required]
        public string Email { get; set; }
        [Required]

        public string Token { get; set; }  // ✅ تأكد من إضافة هذا السطر

        [Required]
        public string NewPassword { get; set; }


        [Required]
        public string ConfirmPassword { get; set; }
    }
}
