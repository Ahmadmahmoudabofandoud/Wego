using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wego.API.Models.DTOS.Flights.Dtos;
using Wego.API.Models.DTOS.Hotels.Dtos;
using Wego.API.Models.DTOS.Locations.Dtos;
using Wego.Core;
using Wego.Core.Models;
using Wego.Core.Models.Flights;
using Wego.Core.Models.Hotels;
using Wego.Core.Specifications.AirlineSpecification;
using Wego.Core.Specifications.LocationSpacification;
using Wego.Core.Specifications.HotelSpecification;
using Swashbuckle.AspNetCore.Annotations;
using Wego.API.Models.DTOS;
using Microsoft.AspNetCore.Identity;
using Wego.Core.Models.Identity;
using Wego.API.Helpers;


namespace Wego.API.Controllers
{
    public class HomeController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly LocationService _locationService;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(IUnitOfWork unitOfWork, IMapper mapper, LocationService locationService, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _locationService = locationService;
            _userManager = userManager;
        }

        [HttpGet("popular-hotels-details(Recommendation)")]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetPopularHotelsWithDetails([FromQuery] HotelSpecParams specParams)
        {
            var spec = new HotelWithDetailsSpecification(specParams);
            var hotels = await _unitOfWork.Repository<Hotel>().GetAllWithSpecAsync(spec) ?? new List<Hotel>();

            if (!hotels.Any())
                return NotFound(new { message = "No popular hotels found." });

            var currentUser = await _userManager.GetUserAsync(User);
            var favoriteHotelIds = currentUser != null
                ? (await _unitOfWork.Repository<Favorite>().GetAsync(f => f.UserId == currentUser.Id && f.HotelId != null))
                    .Select(f => f.HotelId)
                    .ToList()
                : new List<int?>();

            var data = _mapper.Map<List<HotelDto>>(hotels);
            data = data.Select(hotel =>
            {
                hotel.IsFavorite = favoriteHotelIds.Contains(hotel.Id);
                hotel.Images = hotel.Images.Select(image => string.IsNullOrEmpty(image) ? null : $"{Request.Scheme}://{Request.Host.Value}{image}").ToList();
                return hotel;
            }).ToList();

            return Ok(data);
        }

        [HttpGet("nearby")]
        public async Task<ActionResult<Pagination<LocationWithHotelsResponseDto>>> GetNearbyLocations([FromQuery] AppSpecParamsNearby specParams)
        {
            var nearbyLocations = await _locationService.GetNearbyLocationsAsync(specParams);
            var totalCount = nearbyLocations.Count;

            if (nearbyLocations == null || !nearbyLocations.Any())
            {
                return NotFound("No locations found within the specified distance.");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var favoriteLocationIds = currentUser != null
                ? (await _unitOfWork.Repository<Favorite>().GetAsync(f => f.UserId == currentUser.Id && f.LocationId != null))
                    .Where(f => f.LocationId != null)
                    .Select(f => f.LocationId)
                    .ToList()
                : new List<int?>();

            var favoriteHotelIds = currentUser != null
                ? (await _unitOfWork.Repository<Favorite>().GetAsync(f => f.UserId == currentUser.Id && f.HotelId != null))
                    .Where(f => f.HotelId != null)
                    .Select(f => f.HotelId)
                    .ToList()
                : new List<int?>();

            foreach (var locationDto in nearbyLocations)
            {

                locationDto.IsFavorite = favoriteLocationIds.Contains(locationDto.Id);

                locationDto.Hotels = locationDto.Hotels.Select(hotel =>
                {
                    hotel.IsFavorite = favoriteHotelIds.Contains(hotel.Id);
                    hotel.Images = hotel.Images.Select(image => string.IsNullOrEmpty(image) ? null : $"{Request.Scheme}://{Request.Host.Value}{image}").ToList();
                    return hotel;
                }).ToList();
            }

            return Ok(nearbyLocations);
            //return Ok(new Pagination<LocationWithHotelsResponseDto>(specParams.PageIndex, specParams.PageSize, totalCount, nearbyLocations));
        }


        [HttpGet("LocationWithHotelsResponse_ByCityName")]
        public async Task<ActionResult<List<LocationWithHotelsResponseDto>>> GetNearLocations([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest(new { message = "City name is required." });
            }

            var specParams = new AppSpecParamsNearby
            {
                Search = city.Trim(),
                PageIndex = 1,
                PageSize = 10
            };

            var hotelSpecParams = new HotelSpecParams
            {
                Search = specParams.Search,
                PageIndex = specParams.PageIndex,
                PageSize = specParams.PageSize
            };

            var hotelSpec = new HotelWithDetailsSpecification(hotelSpecParams);

            var hotels = await _unitOfWork.Repository<Hotel>()
                .GetAllWithSpecAsync(hotelSpec);


            var locationSpec = new LocationWithAirportsAndHotelsSpecification(specParams);

            var locations = await _unitOfWork.Repository<Location>()
                .GetAllWithSpecAsync(locationSpec);

            if (locations == null || !locations.Any())
            {
                return NotFound(new { message = "No locations found in this city." });
            }

            var locationDtos = _mapper.Map<List<LocationWithHotelsResponseDto>>(locations);

            // Get current user and their favorites
            var currentUser = await _userManager.GetUserAsync(User);

            var favoriteLocationIds = currentUser != null
                ? (await _unitOfWork.Repository<Favorite>().GetAsync(f => f.UserId == currentUser.Id && f.LocationId != null))
                    .Where(f => f.LocationId != null)
                    .Select(f => f.LocationId)
                    .ToList()
                : new List<int?>();

            var favoriteHotelIds = currentUser != null
                ? (await _unitOfWork.Repository<Favorite>().GetAsync(f => f.UserId == currentUser.Id && f.HotelId != null))
                    .Where(f => f.HotelId != null)
                    .Select(f => f.HotelId)
                    .ToList()
                : new List<int?>();

            foreach (var locationDto in locationDtos)
            {
                locationDto.IsFavorite = favoriteLocationIds.Contains(locationDto.Id);

                locationDto.Hotels = locationDto.Hotels.Select(hotel =>
                {
                    hotel.IsFavorite = favoriteHotelIds.Contains(hotel.Id);
                    hotel.Images = hotel.Images.Select(image => string.IsNullOrEmpty(image) ? null : $"{Request.Scheme}://{Request.Host.Value}{image}").ToList();
                    return hotel;
                }).ToList();
            }

            return Ok(locationDtos);
        }


        [HttpGet("LocationWithHotelsResponse_ByLocationId")]
        [SwaggerOperation(Summary = "Get locations by ID", Description = "Retrieve hotels and airports within a specific location based on its ID.")]
        public async Task<ActionResult<LocationWithHotelsResponseDto>> GetLocationById([FromQuery] int locationId)
        {
            if (locationId <= 0)
            {
                return BadRequest(new { message = "A valid location ID is required." });
            }

            var hotelSpecParams = new HotelSpecParams
            {
                LocationId = locationId,
                PageIndex = 1,
                PageSize = 10
            };

            var hotelSpec = new HotelWithDetailsSpecification(hotelSpecParams);

            var hotels = await _unitOfWork.Repository<Hotel>()
                .GetAllWithSpecAsync(hotelSpec);

            var location = await _unitOfWork.Repository<Location>()
                .GetByIdAsync(locationId);

            if (location == null)
            {
                return NotFound(new { message = "No location found with this ID." });
            }

            var currentUser = await _userManager.GetUserAsync(User);

            var favoriteHotelIds = currentUser != null
                ? (await _unitOfWork.Repository<Favorite>().GetAsync(f => f.UserId == currentUser.Id && f.HotelId != null))
                    .Where(f => f.HotelId != null)
                    .Select(f => f.HotelId)
                    .ToList()
                : new List<int?>();

            var favoriteLocationIds = currentUser != null
                ? (await _unitOfWork.Repository<Favorite>().GetAsync(f => f.UserId == currentUser.Id && f.LocationId != null))
                    .Where(f => f.LocationId != null)
                    .Select(f => f.LocationId)
                    .ToList()
                : new List<int?>();

            var locationDto = _mapper.Map<LocationWithHotelsResponseDto>(location);

            locationDto.IsFavorite = favoriteLocationIds.Contains(locationDto.Id);

            locationDto.Hotels = _mapper.Map<List<HotelDto>>(hotels);

            locationDto.Hotels = locationDto.Hotels.Select(hotel =>
            {
                hotel.IsFavorite = favoriteHotelIds.Contains(hotel.Id);
                hotel.Images = hotel.Images.Select(image => string.IsNullOrEmpty(image) ? null : $"{Request.Scheme}://{Request.Host.Value}{image}").ToList();
                return hotel;
            }).ToList();

            return Ok(locationDto);
        }



        [HttpGet("popular-airlines")]
        [SwaggerOperation(Summary = "Get popular airlines", Description = "Retrieve a list of popular airlines with flight details.")]
        public async Task<ActionResult<IEnumerable<AirlineDto>>> GetPopularAirlines([FromQuery] AirlineSpecParams specParams)
        {
            var spec = new AirlineWithAirplanesAndFlightsSpecification(specParams);
            var airlines = await _unitOfWork.Repository<Airline>().GetAllWithSpecAsync(spec) ?? new List<Airline>();

            if (!airlines.Any())
                return NotFound(new { message = "No popular airlines found." });

            return Ok(_mapper.Map<List<AirlineDto>>(airlines));
        }


        [HttpGet("nearby-attractions")]
        [SwaggerOperation(Summary = "Get nearby attractions based on coordinates", Description = "Find attractions near the given GPS coordinates within a specified distance.")]
        public async Task<ActionResult<List<AttractionDto>>> GetNearbyAttractions([FromQuery] AppSpecParamsNearby specParams)
        {
            var spec = new AttractionSpecification(specParams);
            var attractions = await _unitOfWork.Repository<Attraction>().GetAllWithSpecAsync(spec) ?? new List<Attraction>();

            if (!attractions.Any())
                return NotFound(new { message = "No attractions found within the specified distance." });

            var currentUser = await _userManager.GetUserAsync(User);
            var favoriteAttractionIds = currentUser != null
                ? (await _unitOfWork.Repository<Favorite>()
                    .GetAsync(f => f.UserId == currentUser.Id && f.AttractionId != null))
                    .Where(f => f.AttractionId != null)
                    .Select(f => f.AttractionId)
                    .ToList()
                : new List<int?>();

            var data = _mapper.Map<List<AttractionDto>>(attractions);

            data = data.Select(attr =>
            {
                attr.IsFavorite = favoriteAttractionIds.Contains(attr.Id);
                attr.Image = string.IsNullOrEmpty(attr.Image) ? null : $"{Request.Scheme}://{Request.Host.Value}{attr.Image}";
                return attr;
            }).ToList();

            return Ok(data);
        }

    }
}

