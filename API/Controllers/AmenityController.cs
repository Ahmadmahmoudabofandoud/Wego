using API.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wego.API.Helpers;
using Wego.API.Models.DTOS.Hotels.Dtos;
using Wego.Core;
using Wego.Core.Models.Hotels;

namespace Wego.API.Controllers
{
    public class AmenityController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AmenityController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AmenityDto>>> GetAllAmenities()
        {
            var amenities = await _unitOfWork.Repository<Amenity>().GetAllAsync();

            var data = _mapper.Map<IReadOnlyList<Amenity>, IReadOnlyList<AmenityDto>>(amenities);

            data = data.Select(a =>
            {
                a.Image = string.IsNullOrEmpty(a.Image) ? null : $"{Request.Scheme}://{Request.Host.Value}{a.Image}";
                return a;
            }).ToList();

            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AmenityDto>> GetAmenityById(int id)
        {
            var amenity = await _unitOfWork.Repository<Amenity>().GetByIdAsync(id);
            if (amenity == null) return NotFound(new ApiResponse(404));

            var result = _mapper.Map<AmenityDto>(amenity);
            result.Image = string.IsNullOrEmpty(result.Image) ? null : $"{Request.Scheme}://{Request.Host.Value}{result.Image}";

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<AmenityDto>> AddAmenity([FromForm] AmenityPostDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var amenity = _mapper.Map<Amenity>(dto);
            amenity.Image = await ProcessAmenityImageAsync(dto.ImageFile, "amenitiesImg");

            await _unitOfWork.Repository<Amenity>().Add(amenity);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetAmenityById), new { id = amenity.Id }, _mapper.Map<AmenityDto>(amenity));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AmenityDto>> UpdateAmenity(int id, [FromForm] AmenityPutDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.Id) return BadRequest("ID mismatch");

            var amenity = await _unitOfWork.Repository<Amenity>().GetByIdAsync(dto.Id);
            if (amenity == null) return NotFound(new ApiResponse(404));

            _mapper.Map(dto, amenity);
            amenity.Image = await ProcessAmenityImageAsync(dto.Image, "amenitiesImg", amenity.Image);

            _unitOfWork.Repository<Amenity>().Update(amenity);
            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<AmenityDto>(amenity));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAmenity(int id)
        {
            var amenity = await _unitOfWork.Repository<Amenity>().GetByIdAsync(id);
            if (amenity == null) return NotFound(new ApiResponse(404));

            _unitOfWork.Repository<Amenity>().Delete(amenity);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = $"Amenity '{amenity.Name}' has been deleted successfully." });
        }

        private async Task<string?> ProcessAmenityImageAsync(IFormFile? imageFile, string folder, string? existingImage = null)
        {
            if (imageFile == null) return existingImage;

            if (!string.IsNullOrEmpty(existingImage))
            {
                var oldImageName = Path.GetFileName(existingImage);
                ImageHelper.RemoveImage(folder, oldImageName);
            }

            string newImageName = $"amenity-{Guid.NewGuid()}.jpg";
            await ImageHelper.UploadImageAsync(imageFile, folder, newImageName);

            return $"/imgs/{folder}/{newImageName}";
        }
    }
}
