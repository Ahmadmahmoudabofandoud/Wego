using System.ComponentModel.DataAnnotations;

namespace Wego.API.Models.DTOS.Identity
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
