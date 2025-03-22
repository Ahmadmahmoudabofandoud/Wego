using API.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wego.Core.Models.Hotels;
using Wego.Core;
using Wego.API.Models.DTOS.Hotels.Dtos;
using Wego.API.Helpers;
using Wego.Core.Specifications;
using Wego.Core.Specifications.AmenitySpecification;

namespace Wego.API.Controllers
{
    public class AmenitiesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AmenitiesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<AmenityDto>>> GetAllAmenities([FromQuery] AppSpecParams specParams)
        {
            var spec = new AmenityWithDetailsSpecification(specParams);
            var amenities = await _unitOfWork.Repository<Amenity>().GetAllWithSpecAsync(spec);
            var totalCount = await _unitOfWork.Repository<Amenity>().GetCountWithSpecAsync(new AmenityWithFilterationForCountSpecifications(specParams));

            var data = _mapper.Map<IReadOnlyList<Amenity>, IReadOnlyList<AmenityDto>>(amenities);
            return Ok(new Pagination<AmenityDto>(specParams.PageIndex, specParams.PageSize, totalCount, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AmenityDto>> GetAmenityById(int id)
        {
            var spec = new AmenityWithDetailsSpecification(id);
            var amenity = await _unitOfWork.Repository<Amenity>().GetEntityWithSpecAsync(spec);
            if (amenity == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<AmenityDto>(amenity));
        }

        //[HttpPost]
        //public async Task<ActionResult<AmenityDto>> AddAmenity([FromBody] AmenityPostDto dto)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    var amenity = _mapper.Map<Amenity>(dto);
        //    await _unitOfWork.Repository<Amenity>().Add(amenity);
        //    await _unitOfWork.CompleteAsync();

        //    return CreatedAtAction(nameof(GetAmenityById), new { id = amenity.Id }, _mapper.Map<AmenityDto>(amenity));
        //}

        //[HttpPost("assign-to-hotel")]
        //public async Task<ActionResult> AssignAmenityToHotel([FromBody] HotelAmenityPostDto dto)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    var hotel = await _unitOfWork.Repository<Hotel>().GetByIdAsync(dto.HotelId);
        //    var amenity = await _unitOfWork.Repository<Amenity>().GetByIdAsync(dto.AmenityId);

        //    if (hotel == null || amenity == null)
        //        return NotFound(new ApiResponse(404, "Hotel or Amenity not found"));

        //    var hotelAmenity = new HotelAmenity { HotelId = dto.HotelId, AmenityId = dto.AmenityId };
        //    await _unitOfWork.Repository<HotelAmenity>().Add(hotelAmenity);
        //    await _unitOfWork.CompleteAsync();

        //    return Ok(new { message = "Amenity assigned to hotel successfully." });
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult<AmenityDto>> UpdateAmenity(int id, [FromBody] AmenityDto dto)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    if (id != dto.Id) return BadRequest("ID mismatch");

        //    var amenity = await _unitOfWork.Repository<Amenity>().GetByIdAsync(id);
        //    if (amenity == null) return NotFound(new ApiResponse(404));

        //    _mapper.Map(dto, amenity);
        //    _unitOfWork.Repository<Amenity>().Update(amenity);
        //    await _unitOfWork.CompleteAsync();

        //    return Ok(_mapper.Map<AmenityDto>(amenity));
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteAmenity(int id)
        //{
        //    var amenity = await _unitOfWork.Repository<Amenity>().GetByIdAsync(id);
        //    if (amenity == null) return NotFound(new ApiResponse(404));

        //    // التحقق مما إذا كان الـ Amenity مرتبطًا بفنادق
        //    var amenityCount = await _unitOfWork.Repository<HotelAmenity>().GetCountWithSpecAsync(new HotelAmenitySpecification(id));
        //    if (amenityCount > 0)
        //        return BadRequest(new ApiResponse(400, "This Amenity is assigned to a hotel and cannot be deleted."));

        //    _unitOfWork.Repository<Amenity>().Delete(amenity);
        //    await _unitOfWork.CompleteAsync();

        //    return Ok(new { message = $"Amenity '{amenity.Name}' has been deleted successfully." });
        //}

    }
}
