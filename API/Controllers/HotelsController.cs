using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Wego.Core;
using Wego.API.Models.DTOS.Hotels.Dtos;
using Wego.Core.Specifications.HotelSpecification;
using Wego.API.Helpers;
using API.Errors;
using Wego.Core.Models.Hotels;
using Wego.Core.Specifications;
using Wego.Core.Models;

namespace Wego.API.Controllers
{
    public class HotelsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HotelsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<HotelDto>>> GetAllHotels([FromQuery] AppSpecParams specParams)
        {
            var spec = new HotelWithDetailsSpecification(specParams);
            var hotels = await _unitOfWork.Repository<Hotel>().GetAllWithSpecAsync(spec);
            var totalCount = await _unitOfWork.Repository<Hotel>().GetCountWithSpecAsync(new HotelWithFilterationForCountSpecifications(specParams));

            var data = _mapper.Map<IReadOnlyList<Hotel>, IReadOnlyList<HotelDto>>(hotels);
            return Ok(new Pagination<HotelDto>(specParams.PageIndex, specParams.PageSize, totalCount, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HotelDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<HotelDto>> GetHotelById(int id)
        {
            var spec = new HotelWithDetailsSpecification(id);
            var hotel = await _unitOfWork.Repository<Hotel>().GetEntityWithSpecAsync(spec);
            if (hotel == null) return NotFound(new ApiResponse(404));

            var result = _mapper.Map<HotelDto>(hotel);
            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<HotelDto>> AddHotel([FromForm] HotelPostDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var hotel = _mapper.Map<Hotel>(dto);
            hotel.Images = await ProcessHotelImagesAsync(dto.Images, "hotelsImg");

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
                existingImages.Add(new Image { Url = $"/imgs/{folder}/{newImageName}" });
            }

            return existingImages;
        }
    }
}
