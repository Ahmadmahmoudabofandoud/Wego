namespace Wego.API.Models.DTOS.Identity
{
    public class ChangePasswordDto
    {
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
