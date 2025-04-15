using System.ComponentModel.DataAnnotations;
using Wego.Core.Models.Enums;

namespace Wego.API.Models.DTOS.Identity
{
    public class ProfileDto
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public string PassportNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string? ProfileImageUrl { get; set; }
    }

    public class ProfileUpdateDto
    {

        public string DisplayName { get; set; }
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }
        public string? PassportNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public Nationality Nationality { get; set; }
        public Gender? Gender { get; set; }
        public string? Address { get; set; }
        public IFormFile? ProfileImageUrl { get; set; }
    }



}
