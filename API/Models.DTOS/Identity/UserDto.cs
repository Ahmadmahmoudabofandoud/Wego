namespace Wego.API.Models.DTOS.Identity
{
    public class UserDto
    {
        public bool Status { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
