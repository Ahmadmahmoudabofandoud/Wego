using System.ComponentModel.DataAnnotations;

namespace Wego.API.Models.DTOS.Identity
{
    public class VerifyCodeDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
