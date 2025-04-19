using AutoMapper;
using Wego.API.Models.DTOS;
using Wego.API.Models.DTOS.Bookings;
using Wego.API.Models.DTOS.Flights.Dtos;
using Wego.API.Models.DTOS.Hotels.Dtos;
using Wego.API.Models.DTOS.Identity;
using Wego.API.Models.DTOS.Locations.Dtos;
using Wego.API.Models.DTOS.Rooms.Dtos;
using Wego.Core.Models;
using Wego.Core.Models.Booking;
using Wego.Core.Models.Enums;
using Wego.Core.Models.Flights;
using Wego.Core.Models.Hotels;
using Wego.Core.Models.Identity;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Airport Mapping
        CreateMap<Airport, AirportDto>()
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ReverseMap();

        CreateMap<AirportPostDto, Airport>();

        CreateMap<AirportPutDto, Airport>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        #endregion

        #region Airline Mapping
        CreateMap<Airline, AirlineDto>()
            .ForMember(dest => dest.Flights, opt => opt.MapFrom(src => src.Flights.Count))
            .ForMember(dest => dest.Airplanes, opt => opt.MapFrom(src => src.Airplanes.Count))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null ? src.Image : null));

        CreateMap<AirlinePostDto, Airline>();
        CreateMap<AirlinePutDto, Airline>();
        #endregion

        #region Hotel Mapping
        CreateMap<Hotel, HotelDto>()
            .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.City))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.ImageData)))
            .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => src.Amenities))
            .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms))
            .ForMember(dest => dest.Policies, opt => opt.MapFrom(src => src.Policies))
            .ForMember(dest => dest.ReviewIds, opt => opt.MapFrom(src => src.Reviews.Select(r => r.Id).ToList()));

        CreateMap<HotelPostDto, Hotel>()
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.Policies, opt => opt.MapFrom(src => src.Policies));

        CreateMap<HotelPutDto, Hotel>()
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.Policies, opt => opt.MapFrom(src => src.Policies));        
        #endregion

        #region Location Mapping
        CreateMap<Location, LocationsDto>()
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude));

        CreateMap<LocationPostDto, Location>()
            .ForMember(dest => dest.Latitude, opt => opt.Ignore()) 
            .ForMember(dest => dest.Longitude, opt => opt.Ignore());

        CreateMap<LocationPutDto, Location>()
            .ForMember(dest => dest.Latitude, opt => opt.Ignore())
            .ForMember(dest => dest.Longitude, opt => opt.Ignore());

        // خريطة بين LocationWithHotelsResponseDto و Location
        CreateMap<Location, LocationWithHotelsResponseDto>()
            .ForMember(dest => dest.Airports, opt => opt.MapFrom(src => src.Airports))
            .ForMember(dest => dest.Hotels, opt => opt.MapFrom(src => src.Hotels));

        #endregion

        #region Room Mapping

        CreateMap<Room, RoomDto>()
            .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType.ToString()))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(img => img.ImageData).ToList()))
            .ForMember(dest => dest.RoomOptions, opt => opt.MapFrom(src => src.RoomOptions));

        CreateMap<RoomPostDto, Room>()
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.RoomOptions, opt => opt.MapFrom(src => src.RoomOptions));

        CreateMap<RoomPutDto, Room>()
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.RoomOptions, opt => opt.MapFrom(src => src.RoomOptions));

        CreateMap<RoomOption, RoomOptionDto>();

        CreateMap<RoomOptionCreateDto, RoomOption>();
        #endregion


        #region RoomBooking
        CreateMap<RoomBooking, RoomBookingDto>()
            .ForMember(dest => dest.RoomTitle, opt => opt.MapFrom(src => src.Room.RoomTitle))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Booking.Status.ToString()))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Booking.TotalPrice))
            .ForMember(dest => dest.RoomOptionId, opt => opt.MapFrom(src => src.RoomOptionId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Booking.UserId));

        CreateMap<RoomBookingPostDto, RoomBooking>()
            .ForMember(dest => dest.Booking, opt => opt.MapFrom(src => new HotelBooking()))
            .ForMember(dest => dest.RoomOptionId, opt => opt.MapFrom(src => src.RoomOptionId));


        CreateMap<RoomBooking, RoomBookingPutDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Booking.Status.ToString()))
            .ForMember(dest => dest.RoomOptionId, opt => opt.MapFrom(src => src.RoomOptionId)).ReverseMap();

        #endregion

        #region Review Mapping
        CreateMap<Review, ReviewDto>()
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User != null ? src.User.DisplayName : null))
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel != null ? src.Hotel.Name : null))
            .ForMember(dest => dest.AirlineName, opt => opt.MapFrom(src => src.Airline != null ? src.Airline.Name : null));

        CreateMap<ReviewPostDto, Review>()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
        CreateMap<ReviewPutDto, Review>();

        #endregion

        #region Profile Mapping
        CreateMap<AppUser, ProfileDto>()
            .ForMember(dest => dest.ProfileImageUrl, opt => opt.Ignore())
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.IsGuest, opt => opt.MapFrom(src => src.IsGuest)) 
            .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality));

        CreateMap<ProfileUpdateDto, AppUser>()
            .ForMember(dest => dest.ProfileImageUrl, opt => opt.Ignore())
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.PassportNumber, opt => opt.MapFrom(src => src.PassportNumber))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
            .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

        CreateMap<ProfilePostDto, AppUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.DisplayName)) 
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
            .ForMember(dest => dest.PassportNumber, opt => opt.MapFrom(src => src.PassportNumber))
            .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.NationalId))
            .ForMember(dest => dest.TripPurpose, opt => opt.MapFrom(src => src.TripPurpose))
            .ForMember(dest => dest.SpecialNeeds, opt => opt.MapFrom(src => src.SpecialNeeds))
                        .ForMember(dest => dest.IsGuest, opt => opt.MapFrom(src => src.IsGuest)); 
        

        #endregion

        #region Airplane Mapping

        CreateMap<Airplane, AirplaneDto>()
            .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.Feature.Select(f => f.ToString()).ToList()));

        CreateMap<AirplaneDto, Airplane>()
            .ForMember(dest => dest.Feature, opt => opt.MapFrom(src => src.Features.Select(f => Enum.Parse<Features>(f)).ToList()));

        CreateMap<AirplanePostDto, Airplane>()
            .ForMember(dest => dest.Feature, opt => opt.MapFrom(src => src.Features.Select(f => Enum.Parse<Features>(f)).ToList()));

        CreateMap<AirplanePutDto, Airplane>()
            .ForMember(dest => dest.Feature, opt => opt.MapFrom(src => src.Features.Select(f => Enum.Parse<Features>(f)).ToList()));

        #endregion

        #region Flight Mapping
        CreateMap<Flight, FlightDTO>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.AirlineId, opt => opt.MapFrom(src => src.Airplane != null ? src.Airplane.AirlineId : (int?)null))
            .ForMember(dest => dest.NoOfBookings, opt => opt.MapFrom(src => src.FlightBookings != null ? src.FlightBookings.Count : (int?)null))
            .ReverseMap()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (FlightStatus)src.Status));
        CreateMap<Flight, FlightDto>();

        CreateMap<Flight, FlightTicketDTO>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.AirlineName, opt => opt.MapFrom(src => src.Airline != null ? src.Airline.Name : "Unknown"))
            .ForMember(dest => dest.AirplaneType, opt => opt.MapFrom(src => src.Airplane != null ? src.Airplane.Type : "Unknown"))
            .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => CalculateDurationMinutes(src.DepartureTime, src.ArrivalTime)));
        #endregion

        #region Amentity
        // Amenity → AmenityDto
        CreateMap<Amenity, AmenityDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));

        // Dto → Amenity
        CreateMap<AmenityPostDto, Amenity>()
            .ForMember(dest => dest.Image, opt => opt.Ignore());

        CreateMap<AmenityPutDto, Amenity>()
            .ForMember(dest => dest.Image, opt => opt.Ignore());

        #endregion

        #region Favorites
        CreateMap<Favorite, FavoriteDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.HotelId))
            .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId))
            .ForMember(dest => dest.AirlineId, opt => opt.MapFrom(src => src.AirlineId))
            .ReverseMap();

        #endregion

        #region Attractions
        CreateMap<Attraction, AttractionDto>()
            .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.Location != null ? src.Location.Id : (int?)null))
            .ReverseMap();

        CreateMap<Attraction, AttractionPutDto>()
            .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.Location != null ? src.Location.Id : (int?)null))
            .ReverseMap();

        CreateMap<Attraction, AttractionPostDto>()
            .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.Location != null ? src.Location.Id : (int?)null))
            .ReverseMap(); 
        #endregion

    }

    private static string CalculateDurationMinutes(DateTime start, DateTime end)
    {
        TimeSpan duration = end - start;
        return $"{duration.Hours} hr {duration.Minutes:D2} min";
    }
}

