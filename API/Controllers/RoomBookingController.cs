using API.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wego.API.Models.DTOS.Bookings;
using Wego.Core.Models.Booking;
using Wego.Core;
using Wego.Core.Specifications.BookingSpacification;
using Microsoft.AspNetCore.Identity;
using Wego.Core.Models.Identity;
using Wego.API.Controllers;
using Wego.Core.Models.Enums;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Wego.Repository.Data;


namespace API.Controllers
{
    public class RoomBookingController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RoomBookingController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager,ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Add a new room booking", Description = "Add a new room booking by an authenticated user.")]
        public async Task<ActionResult<RoomBookingDto>> AddRoomBooking([FromBody] RoomBookingPostDto bookingDetails)
        {
            try
            {
                if (bookingDetails == null)
                    return BadRequest(new ApiResponse(400, "Booking details are required"));

                var userId = await GetAuthenticatedUserId();

                var room = await _context.Rooms
                    .Include(r => r.RoomOptions)
                    .FirstOrDefaultAsync(r => r.Id == bookingDetails.RoomId);

                if (room == null)
                    return BadRequest(new ApiResponse(400, "Room not found"));

                var roomOption = room.RoomOptions.FirstOrDefault(ro => ro.Id == bookingDetails.RoomOptionId);
                if (roomOption == null)
                    return BadRequest(new ApiResponse(400, $"Room option with ID {bookingDetails.RoomOptionId} not found"));

                var duration = (bookingDetails.Checkout - bookingDetails.Checkin).Days;
                if (duration <= 0)
                    return BadRequest(new ApiResponse(400, "Invalid check-in and check-out dates"));

                var booking = _mapper.Map<RoomBooking>(bookingDetails);
                booking.RoomOption = roomOption;
                booking.Room = room;

                booking.Booking = new HotelBooking
                {
                    UserId = userId,
                    TotalPrice = duration * roomOption.Price
                };

                _unitOfWork.Repository<RoomBooking>().Add(booking);
                await _unitOfWork.CompleteAsync();

                if (booking.Id == 0)
                    return BadRequest(new ApiResponse(400, "Failed to create room booking"));

                var bookingDto = _mapper.Map<RoomBookingDto>(booking);
                return Ok(bookingDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse(401, ex.Message));
            }
        }




        // Get Room Reservations by RoomId and Date Range (Checkin & Checkout)
        [HttpGet("RoomReserve")]
        [SwaggerOperation(Summary = "Get room bookings by room ID and date range", Description = "Get bookings for a specific room during a date range.")]
        public async Task<ActionResult<IReadOnlyList<RoomBookingDto>>> GetRoomBookingsByRoomId([FromQuery] int roomId, [FromQuery] DateTime checkin, [FromQuery] DateTime checkout)
        {
            var spec = new RoomBookingWithDetailsSpecification(new RoomBookingSpecParams
            {
                Search = roomId.ToString(),
                Checkin = checkin,
                Checkout = checkout
            });

            var bookings = await _unitOfWork.Repository<RoomBooking>().GetAllWithSpecAsync(spec);
            var data = _mapper.Map<IReadOnlyList<RoomBooking>, IReadOnlyList<RoomBookingDto>>(bookings);

            return Ok(data);
        }


        // Get User's Bookings (Confirmed only)
        [HttpGet("user-booking")]
        [Authorize]
        [SwaggerOperation(Summary = "Get user's confirmed bookings", Description = "Get a list of confirmed bookings for the authenticated user.")]
        public async Task<ActionResult<IReadOnlyList<RoomBookingDto>>> GetUserBookings()
        {
            try
            {
                var userId = await GetAuthenticatedUserId();
                var spec = new RoomBookingWithDetailsSpecification(new RoomBookingSpecParams { UserId = userId, Status = BookingStatus.Confirmed });
                var bookings = await _unitOfWork.Repository<RoomBooking>().GetAllWithSpecAsync(spec);
                var data = _mapper.Map<IReadOnlyList<RoomBooking>, IReadOnlyList<RoomBookingDto>>(bookings);

                return Ok(data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse(401, ex.Message));
            }
        }


        // Delete Room Booking

        [HttpDelete("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Delete an existing room booking", Description = "Delete a room booking by ID.")]
        public async Task<ActionResult> DeleteRoomBooking(int id)
        {
            try
            {
                var userId = await GetAuthenticatedUserId();
                var booking = await _unitOfWork.Repository<RoomBooking>().GetByIdAsync(id);

                if (booking == null)
                    return NotFound(new ApiResponse(404, "Booking not found"));

                if (booking.Booking.UserId != userId)
                    return Unauthorized(new ApiResponse(403, "You can only delete your own bookings"));

                _unitOfWork.Repository<RoomBooking>().Delete(booking);
                await _unitOfWork.CompleteAsync();

                return NoContent(); // Return No Content if successful
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse(401, ex.Message));
            }
        }


        // Helper function to check if user is authenticated and fetch userId
        private async Task<string> GetAuthenticatedUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Get the user ID from the claims
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User is not authenticated");

            return userId;
        }


        //// Helper method to get all dates between Checkin and Checkout
        //private IEnumerable<DateTime> GetDatesInRange(DateTime checkin, DateTime checkout)
        //{
        //    for (var date = checkin.Date; date <= checkout.Date; date = date.AddDays(1))
        //    {
        //        yield return date;
        //    }
        //}
    }
}
