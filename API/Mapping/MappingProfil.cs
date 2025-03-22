using AutoMapper;
using Wego.API.Models.DTOS.Flights.Dtos;
using Wego.API.Models.DTOS.Hotels.Dtos;
using Wego.API.Models.DTOS.Identity;
using Wego.API.Models.DTOS.Locations.Dtos;
using Wego.API.Models.DTOS.Rooms.Dtos;
using Wego.Core.Models;
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
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Images != null ? src.Images.Url : null));
        #endregion

        #region Hotel Mapping
        CreateMap<Hotel, HotelDto>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(img => img.Url).ToList()))
            .ReverseMap();

        CreateMap<HotelPostDto, Hotel>()
            .ForMember(dest => dest.Images, opt => opt.Ignore());

        CreateMap<HotelPutDto, Hotel>();

        CreateMap<Hotel, HotelDto>()
            .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.City))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.Url).ToList()));
        #endregion

        #region Location Mapping
        CreateMap<Location, LocationsDto>();

        CreateMap<LocationPostDto, Location>();

        CreateMap<LocationPutDto, Location>();

        CreateMap<Location, LocationWithHotelsResponseDto>()
            .ForMember(dest => dest.Hotels, opt => opt.MapFrom(src => src.Hotels));
        #endregion

        #region Room Mapping
        CreateMap<Room, RoomDto>()
            .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType.ToString()))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(img => img.Url).ToList()));

        // **Mapping RoomPostDto → Room**
        CreateMap<RoomPostDto, Room>()
            .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => Enum.Parse<RoomType>(src.RoomType)))
            .ForMember(dest => dest.Images, opt => opt.Ignore()); // الصور سيتم معالجتها يدويًا قبل الحفظ

        // **Mapping RoomPutDto → Room**
        CreateMap<RoomPutDto, Room>()
            .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => Enum.Parse<RoomType>(src.RoomType)))
            .ForMember(dest => dest.Images, opt => opt.Ignore()); // الصور سيتم تعديلها يدويًا أثناء التحديث

        #endregion


        CreateMap<AppUser, ProfileDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.LockoutEnd == null)) // Status يعتمد على حالة القفل
            .ForMember(dest => dest.Token, opt => opt.Ignore()); // لا يتم تضمين التوكن في الماب مباشرة

        // Mapping from ProfileUpdateDto to AppUser
        CreateMap<ProfileUpdateDto, AppUser>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // لا يمكن تحديث الـ ID
    }
}
