using System.ComponentModel.DataAnnotations;

namespace Wego.API.Models.DTOS.Identity
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
