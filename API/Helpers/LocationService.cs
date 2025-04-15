using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wego.API.Models.DTOS.Locations.Dtos;
using Wego.Core;
using Wego.Core.Models;
using Wego.Core.Models.Flights;
using Wego.Core.Models.Hotels;
using Wego.Core.Services.Contract;
using Wego.Core.Specifications.AirportSpecification;
using Wego.Core.Specifications.HotelSpecification;
using Wego.Core.Specifications.LocationSpacification;
using Wego.Repository.Data;

public class LocationService : ILocationService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public LocationService(IMapper mapper, ApplicationDbContext context, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _context = context;
        _unitOfWork = unitOfWork;
    }
    public async Task<IReadOnlyList<LocationWithHotelsResponseDto>> GetNearbyLocationsAsync(AppSpecParamsNearby specParams)
    {
        var hotelSpecParams = new HotelSpecParams
        {
            Search = specParams.Search,
            Sort = specParams.Sort,
            PageIndex = specParams.PageIndex,
            PageSize = specParams.PageSize
        };
        var hotelSpec = new HotelWithDetailsSpecification(hotelSpecParams);
        var airportSpec = new AirportWithLocationSpecification(specParams);

        // جلب الفنادق والمطارات
        var hotels = await _unitOfWork.Repository<Hotel>()
            .GetAllWithSpecAsync(hotelSpec);

        var airports = await _unitOfWork.Repository<Airport>()
            .GetAllWithSpecAsync(airportSpec);

        // تحديد المعايير لجلب المواقع مع الفنادق والمطارات
        var locationSpec = new LocationWithAirportsAndHotelsSpecification(specParams);

        // جلب المواقع التي تحتوي على الفنادق والمطارات
        var locations = await _unitOfWork.Repository<Location>()
            .GetAllWithSpecAsync(locationSpec);

        // تصفية المواقع حسب المسافة
        var nearbyLocations = locations
            .Where(loc =>
                loc.Latitude.HasValue && loc.Longitude.HasValue &&
                CalculateDistance(
                    specParams.Latitude,
                    specParams.Longitude,
                    loc.Latitude.Value,
                    loc.Longitude.Value
                ) <= specParams.MaxDistance
            )
            .ToList();


        // تحويل المواقع إلى DTOs باستخدام AutoMapper
        var locationDtos = _mapper.Map<List<LocationWithHotelsResponseDto>>(nearbyLocations);

        // إعادة المواقع القريبة
        return locationDtos;
    }

    public async Task<IReadOnlyList<Attraction>> GetNearbyAttractionsAsync(AppSpecParamsNearby specParams)
    {
        var attractions = await _context.Attractions
            .Where(a => a.Latitude != null && a.Longitude != null)
            .ToListAsync();

        // تصفية المعالم السياحية حسب المسافة
        var nearbyAttractions = attractions
            .Where(attraction =>
                CalculateDistance(
                    specParams.Latitude,
                    specParams.Longitude,
                    attraction.Latitude.Value,
                    attraction.Longitude.Value
                ) <= specParams.MaxDistance
            )
            .ToList();

        return nearbyAttractions;
    }

    public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double dlon = Radians(lon2 - lon1);
        double dlat = Radians(lat2 - lat1);

        double a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
                   Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) *
                   Math.Sin(dlon / 2) * Math.Sin(dlon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        double R = 6378.16;  
        return R * c;  
    }

    public static double Radians(double degrees)
    {
        // تحويل الدرجات إلى راديان
        return degrees * Math.PI / 180;
    }
}