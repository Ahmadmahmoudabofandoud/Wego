namespace Wego.API.Models.DTOS.Identity
{
    public class ProfileDto
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PassportNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; } // يمكن استخدامها لتحديد إذا كان الحساب نشطًا أم لا
        public string Token { get; set; } // يمكن استخدامها في حالة الحاجة إلى إرسال توكن مع البيانات
    }

    public class ProfileUpdateDto
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PassportNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }

}
