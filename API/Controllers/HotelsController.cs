using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Wego.Core;
using Wego.API.Models.DTOS.Hotels.Dtos;
using Wego.Core.Specifications.HotelSpecification;
using Wego.API.Helpers;
using API.Errors;
using Wego.Core.Models.Hotels;
using Wego.Core.Models;
using Wego.Core.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Wego.Core.Models.Identity;
using Wego.API.Models.DTOS.Locations.Dtos;
using Wego.Core.Models.Booking;
using Wego.Core.Specifications.BookingSpacification;
using Wego.Core.Specifications.LocationSpacification;
using Wego.Core.Specifications.RoomSpecification;

namespace Wego.API.Controllers
{
    public class HotelsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public HotelsController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(HotelDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<Pagination<HotelDto>>> GetAllHotels([FromQuery] HotelSpecParams specParams)
        {
            var spec = new HotelWithDetailsSpecification(specParams);
            var hotels = await _unitOfWork.Repository<Hotel>().GetAllWithSpecAsync(spec);
            var totalCount = await _unitOfWork.Repository<Hotel>().GetCountWithSpecAsync(new HotelWithFilterationForCountSpecifications(specParams));

            var currentUser = await _userManager.GetUserAsync(User);
            var favoriteHotelIds = currentUser != null
                ? (await _unitOfWork.Repository<Favorite>().GetAsync(f => f.UserId == currentUser.Id && f.HotelId != null))
                    .Select(f => f.HotelId)
                    .ToList()
                : new List<int?>();

            var data = _mapper.Map<IReadOnlyList<Hotel>, IReadOnlyList<HotelDto>>(hotels);
            data = data.Select(h =>
            {
                h.IsFavorite = favoriteHotelIds.Contains(h.Id);
                h.Images = h.Images.Select(image => string.IsNullOrEmpty(image) ? null : $"{Request.Scheme}://{Request.Host.Value}{image}").ToList();
                return h;
            }).ToList();

            return Ok(new Pagination<HotelDto>(specParams.PageIndex, specParams.PageSize, totalCount, data));
        }

        [HttpGet("RoomAvailableByCity")]
        [ProducesResponseType(typeof(LocationWithHotelsResponseDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<List<LocationWithHotelsResponseDto>>> CheckRoomAvailabilityByCity(
            [FromQuery] string city,
            [FromQuery] DateTime checkin,
            [FromQuery] DateTime checkout,
            [FromQuery] int? guests,
            [FromQuery] int? children)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest(new ApiResponse(400, "City name is required"));

            var locationSpecParams = new AppSpecParamsNearby
            {
                Search = city.Trim(),
                PageIndex = 1,
                PageSize = 50
            };

            var locationSpec = new LocationWithAirportsAndHotelsSpecification(locationSpecParams);
            var locations = await _unitOfWork.Repository<Location>().GetAllWithSpecAsync(locationSpec);

            if (locations == null || !locations.Any())
                return NotFound(new ApiResponse(404, "No locations found in this city."));

            var hotelIds = locations
                .SelectMany(l => l.Hotels)
                .Select(h => h.Id)
                .ToList();

            if (!hotelIds.Any())
                return NotFound(new ApiResponse(404, "No hotels found in this city."));

            var rooms = await _unitOfWork.Repository<Room>()
                .GetAllWithSpecAsync(new RoomByHotelIdsSpecification(hotelIds));

            var roomIds = rooms.Select(r => r.Id).ToList();

            if (!roomIds.Any())
                return NotFound(new ApiResponse(404, "No rooms found in the hotels of this city."));

            var spec = new RoomBookingWithDetailsSpecification(new RoomBookingSpecParams
            {
                RoomIds = roomIds,
                Checkin = checkin,
                Checkout = checkout,
                Status = BookingStatus.Confirmed,
                Guests = guests,
                Children = children
            });

            var bookings = await _unitOfWork.Repository<RoomBooking>().GetAllWithSpecAsync(spec);
            var locationDtos = _mapper.Map<List<LocationWithHotelsResponseDto>>(locations);

            if (bookings.Any())
                return BadRequest(new ApiResponse(400, "All rooms in this city are booked during the selected dates."));

            return Ok(locationDtos);
        }




        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HotelDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<HotelDto>> GetHotelById(int id)
        {
            var spec = new HotelWithDetailsSpecification(id);
            var hotel = await _unitOfWork.Repository<Hotel>().GetEntityWithSpecAsync(spec);
            if (hotel == null) return NotFound(new ApiResponse(404));


            var currentUser = await _userManager.GetUserAsync(User);
            var isFavorite = currentUser != null
                ? (await _unitOfWork.Repository<Favorite>().GetAsync(f => f.UserId == currentUser.Id && f.HotelId == id))
                    .Any()
                : false;

            var result = _mapper.Map<HotelDto>(hotel);
            result.IsFavorite = isFavorite;
            result.Images = result.Images.Select(image => string.IsNullOrEmpty(image) ? null : $"{Request.Scheme}://{Request.Host.Value}{image}").ToList();

            return Ok(result);
        }



        [HttpPost]
        public async Task<ActionResult<HotelDto>> AddHotel([FromForm] HotelPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotel = _mapper.Map<Hotel>(dto);

            hotel.Images = await ProcessHotelImagesAsync(dto.Images, "hotelsImg");

            var amenities = await _unitOfWork.Repository<Amenity>().GetAsync(a => dto.AmenityIds.Contains(a.Id));
            hotel.Amenities = amenities.ToList();

            await _unitOfWork.Repository<Hotel>().Add(hotel);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetHotelById), new { id = hotel.Id }, _mapper.Map<HotelDto>(hotel));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<HotelDto>> UpdateHotel(int id, [FromForm] HotelPutDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.Id) return BadRequest("ID mismatch");

            var hotel = await _unitOfWork.Repository<Hotel>().GetByIdAsync(dto.Id);
            if (hotel == null) return NotFound(new ApiResponse(404));

            _mapper.Map(dto, hotel);
            hotel.Images = await ProcessHotelImagesAsync(dto.NewImages, "hotelsImg", hotel.Images?.ToList(), dto.ImagesToDelete);
            var amenities = await _unitOfWork.Repository<Amenity>().GetAsync(a => dto.AmenityIds.Contains(a.Id));
            hotel.Amenities = amenities.ToList();
            _unitOfWork.Repository<Hotel>().Update(hotel);
            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<HotelDto>(hotel));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHotel(int id)
        {
            var hotel = await _unitOfWork.Repository<Hotel>().GetByIdAsync(id);
            if (hotel == null) return NotFound(new ApiResponse(404));

            _unitOfWork.Repository<Hotel>().Delete(hotel);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = $"Hotel '{hotel.Name}' has been deleted successfully." });
        }

        private async Task<List<Image>> ProcessHotelImagesAsync(List<IFormFile>? imageFiles, string folder, List<Image>? existingImages = null, List<int>? imagesToDelete = null)
        {
            existingImages ??= new List<Image>();

            if (imagesToDelete != null)
            {
                existingImages.RemoveAll(img => imagesToDelete.Contains(img.Id));
            }

            if (imageFiles == null || !imageFiles.Any())
                return existingImages;

            foreach (var file in imageFiles)
            {
                string newImageName = $"Hotel-{Guid.NewGuid()}.jpg";
                await ImageHelper.UploadImageAsync(file, folder, newImageName);
                existingImages.Add(new Image { ImageData = $"/imgs/{folder}/{newImageName}" });
            }

            return existingImages;
        }
    }
}
