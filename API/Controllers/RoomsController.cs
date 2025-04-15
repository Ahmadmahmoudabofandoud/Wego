using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Wego.Core;
using Wego.API.Models.DTOS.Rooms.Dtos;
using Wego.Core.Specifications.RoomSpecification;
using API.Errors;
using Wego.Core.Models.Hotels;
using Wego.Core.Specifications;
using Wego.Core.Models;
using Wego.API.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using Wego.API.Models.DTOS.Hotels.Dtos;
using Wego.Core.Specifications.HotelSpecification;
using Wego.Service;
using Wego.Repository.Migrations;

namespace Wego.API.Controllers
{
    public class RoomsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AmenityService _amenityService;

        public RoomsController(IUnitOfWork unitOfWork, IMapper mapper, AmenityService amenityService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _amenityService = amenityService;
        }

        [HttpGet("all-rooms-details")]
        [SwaggerOperation(Summary = "Get all rooms with details", Description = "Retrieve a list of all rooms along with additional details.")]
        public async Task<ActionResult<Pagination<RoomDto>>> GetAllRoomsWithDetails([FromQuery] RoomSpecParams specParams)
        {
            var spec = new RoomWithDetailsSpecification(specParams); 
            var rooms = await _unitOfWork.Repository<Room>().GetAllWithSpecAsync(spec);

            var totalCount = await _unitOfWork.Repository<Room>().GetCountWithSpecAsync(new RoomWithFilterationForCountSpecifications(specParams));

            var data = _mapper.Map<IReadOnlyList<Room>, IReadOnlyList<RoomDto>>(rooms); 

            return Ok(new Pagination<RoomDto>(specParams.PageIndex, specParams.PageSize, totalCount, data)); 
        }



        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoomDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<RoomDto>> GetRoomById(int id)
        {
            var spec = new RoomWithDetailsSpecification(id);
            var room = await _unitOfWork.Repository<Room>().GetEntityWithSpecAsync(spec);
            if (room == null) return NotFound(new ApiResponse(404));

            var result = _mapper.Map<RoomDto>(room);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<RoomDto>> AddRoom([FromForm] RoomPostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var room = _mapper.Map<Room>(dto);
            if (dto.Images != null && dto.Images.Any())
            {
                room.Images = await ProcessRoomImagesAsync(dto.Images, "roomsImg");
            }

            var amenities = await _unitOfWork.Repository<Amenity>().GetAsync(a => dto.AmenityIds.Contains(a.Id));
            room.Amenities = amenities.ToList();
           
            var roomOptions = dto.RoomOptions.Select(optionDto =>
            {
                var roomOption = _mapper.Map<RoomOption>(optionDto);
                roomOption.Room = room;
                return roomOption;
            }).ToList();

            room.RoomOptions = roomOptions;
            await _unitOfWork.Repository<Room>().Add(room);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetRoomById), new { id = room.Id }, _mapper.Map<RoomDto>(room));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RoomDto>> UpdateRoom(int id, [FromForm] RoomPutDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.Id) return BadRequest("ID mismatch");

            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(dto.Id);
            if (room == null) return NotFound(new ApiResponse(404));

            _mapper.Map(dto, room);
            room.Images = await ProcessRoomImagesAsync(dto.NewImages, "roomsImg", room.Images?.ToList(), dto.ImagesToDelete);

            var amenities = await _unitOfWork.Repository<Amenity>().GetAsync(a => dto.AmenityIds.Contains(a.Id));
            room.Amenities = amenities.ToList();

            var roomOptions = new List<RoomOption>();
            foreach (var optionDto in dto.RoomOptions)
            {
                var roomOption = _mapper.Map<RoomOption>(optionDto); 
                roomOptions.Add(roomOption);
            }

            await _unitOfWork.Repository<RoomOption>().AddRange(roomOptions);
            room.RoomOptions = roomOptions;
            _unitOfWork.Repository<Room>().Update(room);
            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<RoomDto>(room));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoom(int id)
        {
            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(id);
            if (room == null) return NotFound(new ApiResponse(404));

            _unitOfWork.Repository<Room>().Delete(room);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = $"Room '{room.RoomTitle}' has been deleted successfully." });
        }

        private async Task<List<Image>> ProcessRoomImagesAsync(List<IFormFile>? imageFiles, string folder, List<Image>? existingImages = null, List<int>? imagesToDelete = null)
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
                string newImageName = $"Room-{Guid.NewGuid()}.jpg";
                await ImageHelper.UploadImageAsync(file, folder, newImageName);
                existingImages.Add(new Image { ImageData = $"/imgs/{folder}/{newImageName}" });
            }
            return existingImages;
        }
    }
}
