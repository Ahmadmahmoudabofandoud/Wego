using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wego.Core.Models.Flights;
using Wego.Core.Models.Hotels;
using Wego.Core.Models.Identity;
using Wego.Core.Models;
using Wego.Core.Specifications;
using API.Errors;
using AutoMapper;
using Wego.API.Helpers;
using Wego.Core.Specifications.ReviewSpacification;
using Wego.Core;
using Wego.API.Models.DTOS;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Wego.API.Controllers
{
    public class ReviewController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ReviewDto>>> GetAllReviews([FromQuery] AppSpecParams specParams)
        {
            var spec = new ReviewWithUserAndHotelSpecification(specParams);
            var reviews = await _unitOfWork.Repository<Review>().GetAllWithSpecAsync(spec);
            var totalCount = await _unitOfWork.Repository<Review>().GetCountWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Review>, IReadOnlyList<ReviewDto>>(reviews);

            return Ok(new Pagination<ReviewDto>(specParams.PageIndex, specParams.PageSize, totalCount, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReviewDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<ReviewDto>> GetReviewById(int id)
        {
            var spec = new ReviewWithUserAndHotelSpecification(id);
            var review = await _unitOfWork.Repository<Review>().GetEntityWithSpecAsync(spec);
            if (review == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<ReviewDto>(review));
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<ActionResult<IReadOnlyList<ReviewDto>>> GetReviewsByHotelId(int hotelId)
        {
            var spec = new ReviewByHotelIdSpecification(hotelId);
            var reviews = await _unitOfWork.Repository<Review>().GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Review>, IReadOnlyList<ReviewDto>>(reviews);

            return Ok(data);
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ReviewDto>> AddReview([FromBody] ReviewPostDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new ApiResponse(401, "User not authorized"));

            if ((dto.HotelId != null && dto.AirlineId != null) ||
                (dto.HotelId == null && dto.AirlineId == null))
            {
                return BadRequest("You must only choose a hotel or airline, not both.");
            }

            var review = _mapper.Map<Review>(dto);
            review.UserId = userId;
            review.ReviewDate = DateTime.UtcNow;

            await _unitOfWork.Repository<Review>().Add(review);
            await _unitOfWork.CompleteAsync();

            var spec = new ReviewWithUserAndHotelSpecification(review.Id);
            var reviewWithDetails = await _unitOfWork.Repository<Review>().GetEntityWithSpecAsync(spec);

            if (reviewWithDetails == null)
                return NotFound(new ApiResponse(404));

            return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, _mapper.Map<ReviewDto>(reviewWithDetails));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReviewDto>> UpdateReview(int id, [FromBody] ReviewPutDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.Id) return BadRequest("ID mismatch");

            var review = await _unitOfWork.Repository<Review>().GetByIdAsync(dto.Id);
            if (review == null) return NotFound(new ApiResponse(404));

            _mapper.Map(dto, review);

            _unitOfWork.Repository<Review>().Update(review);
            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<ReviewDto>(review));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReview(int id)
        {
            var review = await _unitOfWork.Repository<Review>().GetByIdAsync(id);
            if (review == null) return NotFound(new ApiResponse(404));

            _unitOfWork.Repository<Review>().Delete(review);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Review deleted successfully." });
        }
    }

}
