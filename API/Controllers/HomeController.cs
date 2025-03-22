using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wego.API.Models.DTOS.Flights.Dtos;
using Wego.API.Models.DTOS.Hotels.Dtos;
using Wego.API.Models.DTOS.Locations.Dtos;
using Wego.Core;
using Wego.Core.Models;
using Wego.Core.Models.Flights;
using Wego.Core.Models.Hotels;
using Wego.Core.Specifications;
using Wego.Core.Specifications.AirlineSpecification;
using Wego.Core.Specifications.LocationSpacification;
using Wego.Core.Specifications.HotelSpecification;


namespace Wego.API.Controllers
{
    public class HomeController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] AppSpecParams specParams, string sortOrder = "asc")
        {
            // تحقق من query
            if (string.IsNullOrWhiteSpace(specParams.Search) || specParams.Search.Length < 2)
                return BadRequest(new { message = "Search query must be at least 2 characters long." });

            sortOrder = sortOrder?.ToLower() == "desc" ? "desc" : "asc";

            try
            {
                // استخدام الـ Specification مع AppSpecParams
                var locationSpec = new SearchSpecification<Location>(specParams.Search, x => x.City, x => x.Country, sortOrder);
                var hotelSpec = new SearchSpecification<Hotel>(specParams.Search, x => x.Name, sortOrder);
                var airlineSpec = new SearchSpecification<Airline>(specParams.Search, x => x.Name, sortOrder);

                // الحصول على البيانات من الـ Repository
                var locations = await _unitOfWork.Repository<Location>().GetAllWithSpecAsync(locationSpec) ?? new List<Location>();
                var hotels = await _unitOfWork.Repository<Hotel>().GetAllWithSpecAsync(hotelSpec) ?? new List<Hotel>();
                var airlines = await _unitOfWork.Repository<Airline>().GetAllWithSpecAsync(airlineSpec) ?? new List<Airline>();

                // تجهيز النتيجة
                var result = new
                {
                    Locations = _mapper.Map<List<LocationsDto>>(locations),
                    Hotels = _mapper.Map<List<HotelDto>>(hotels),
                    Airlines = _mapper.Map<List<AirlineDto>>(airlines)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }

        [HttpGet("popular-hotels-details")]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetPopularHotelsWithDetails([FromQuery] AppSpecParams specParams)
        {
            var spec = new HotelWithDetailsSpecification(specParams);
            var hotels = await _unitOfWork.Repository<Hotel>().GetAllWithSpecAsync(spec) ?? new List<Hotel>();

            if (!hotels.Any())
                return NotFound(new { message = "No popular hotels found." });

            return Ok(_mapper.Map<List<HotelDto>>(hotels));
        }

        [HttpGet("near-location")]
        public async Task<ActionResult<IEnumerable<LocationsDto>>> GetNearLocations([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest(new { message = "City name is required." });

            var specParams = new AppSpecParams
            {
                Search = city.Trim(),  // البحث بالكلمة المفتاحية
                PageIndex = 1,         // الصفحة الأولى
                PageSize = 10          // عدد النتائج في الصفحة
            };

            var locationSpec = new LocationWithAirportsAndHotelsSpecification(specParams);
            var locations = await _unitOfWork.Repository<Location>().GetAllWithSpecAsync(locationSpec);

            if (locations == null || !locations.Any())
                return NotFound(new { message = "No locations found in this city." });

            return Ok(_mapper.Map<List<LocationsDto>>(locations));
        }

        [HttpGet("popular-airlines")]
        public async Task<ActionResult<IEnumerable<AirlineDto>>> GetPopularAirlines([FromQuery] AppSpecParams specParams)
        {
            var spec = new AirlineWithAirplanesAndFlightsSpecification(specParams);
            var airlines = await _unitOfWork.Repository<Airline>().GetAllWithSpecAsync(spec) ?? new List<Airline>();

            if (!airlines.Any())
                return NotFound(new { message = "No popular airlines found." });

            return Ok(_mapper.Map<List<AirlineDto>>(airlines));
        }
    }


}

