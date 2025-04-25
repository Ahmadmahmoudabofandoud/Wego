using System.ComponentModel.DataAnnotations;
using Wego.Core.Models.Enums;

namespace Wego.API.Models.DTOS.Identity
{
    public class ProfileDto
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }
        public string Email { get; set; }
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
        public DateTime? DateOfBirth { get; set; }

        public string? Nationality { get; set; }
        public Gender? Gender { get; set; }
        public string? Address { get; set; }
        public IFormFile? ProfileImageUrl { get; set; }
    }

    public class ProfilePostDto
    {
        public string DisplayName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string PassportNumber { get; set; }
        public string? Nationality { get; set; }
        public Gender? Gender { get; set; }
        public bool IsGuest { get; set; }

        public string NationalId { get; set; }
        public TripPurpose? TripPurpose { get; set; }
        public string? SpecialNeeds { get; set; }
    }

    public class ProfileBookingDto
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PassportNumber { get; set; }
        public string Nationality { get; set; }
        public string IsGuest { get; set; }
        public string NationalId { get; set; }
        public string? TripPurpose { get; set; } 
        public string? SpecialNeeds { get; set; }
    }

}
