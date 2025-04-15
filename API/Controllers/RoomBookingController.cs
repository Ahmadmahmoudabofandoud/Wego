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
using Wego.Core.Models.Hotels;
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

        // Check if Room is Available
        [HttpGet("RoomAvailable")]
        [SwaggerOperation(Summary = "Check if a room is available during a date range", Description = "Check if there are confirmed bookings for a room during the date range.")]
        public async Task<ActionResult> CheckRoomAvailability([FromQuery] int roomId, [FromQuery] DateTime checkin, [FromQuery] DateTime checkout)
        {
            var spec = new RoomBookingWithDetailsSpecification(new RoomBookingSpecParams
            {
                Search = roomId.ToString(),
                Checkin = checkin,
                Checkout = checkout,
                Status = BookingStatus.Confirmed
            });

            var bookings = await _unitOfWork.Repository<RoomBooking>().GetAllWithSpecAsync(spec);
            if (bookings.Any())
                return BadRequest(new ApiResponse(400, "The room is already booked during the selected dates"));

            return Ok(new ApiResponse(200, "The room is available during the selected dates"));
        }


        // Get Reserved Dates for a Room
        [HttpGet("reservedDates")]
        [SwaggerOperation(Summary = "Get reserved dates for a specific room", Description = "Get the list of reserved dates for a room.")]
        public async Task<ActionResult<IReadOnlyList<DateTime>>> GetReservedDates([FromQuery] int roomId)
        {
            var spec = new RoomBookingWithDetailsSpecification(new RoomBookingSpecParams { Search = roomId.ToString(), Status = BookingStatus.Confirmed });
            var bookings = await _unitOfWork.Repository<RoomBooking>().GetAllWithSpecAsync(spec);

            var reservedDates = bookings
                .SelectMany(b => GetDatesInRange(b.Checkin, b.Checkout))
                .Distinct()
                .ToList();

            return Ok(reservedDates);
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


        //[HttpPut("update-user-booking")]
        //[Authorize]
        //[SwaggerOperation(Summary = "Update an existing room booking", Description = "Update an existing room booking details.")]
        //public async Task<ActionResult<RoomBookingDto>> UpdateRoomBooking([FromBody] RoomBookingPutDto bookingDetails)
        //{
        //    try
        //    {
        //        if (bookingDetails == null)
        //            return BadRequest(new ApiResponse(400, "Booking details are required"));

        //        var userId = await GetAuthenticatedUserId();

        //        var booking = await _unitOfWork.Repository<RoomBooking>().GetByIdAsync(bookingDetails.Id);
        //        if (booking == null)
        //            return NotFound(new ApiResponse(404, "Booking not found"));

        //        //if (booking.Booking == null || booking.Booking.UserId != userId)
        //        //    return Unauthorized(new ApiResponse(403, "You can only update your own bookings"));

        //        var room = await _context.Rooms
        //            .Include(r => r.RoomOptions)
        //            .FirstOrDefaultAsync(r => r.Id == bookingDetails.RoomId);

        //        if (room == null)
        //            return BadRequest(new ApiResponse(400, "Room not found"));

        //        var roomOption = room.RoomOptions.FirstOrDefault(ro => ro.Id == bookingDetails.RoomOptionId);
        //        if (roomOption == null)
        //            return BadRequest(new ApiResponse(400, $"Room option with ID {bookingDetails.RoomOptionId} not found"));

        //        var duration = (bookingDetails.Checkout - bookingDetails.Checkin).Days;
        //        if (duration <= 0)
        //            return BadRequest(new ApiResponse(400, "Invalid check-in and check-out dates"));

        //        // Update room and room option references
        //        booking.Room = room;
        //        booking.RoomOption = roomOption;

        //        // Update main booking details
        //        booking.Checkin = bookingDetails.Checkin;
        //        booking.Checkout = bookingDetails.Checkout;
        //        booking.Guests = bookingDetails.Guests;
        //        booking.Children = bookingDetails.Children;
        //        booking.RoomId = bookingDetails.RoomId;
        //        // Try parsing the status string to the enum
        //        if (!string.IsNullOrEmpty(bookingDetails.Status) &&
        //            Enum.TryParse<BookingStatus>(bookingDetails.Status, true, out var parsedStatus))
        //        {
        //            booking.Booking.Status = parsedStatus;
        //        }
        //        else
        //        {
        //            booking.Booking.Status = null;
        //        }
        //        booking.RoomOptionId = bookingDetails.RoomOptionId;

        //        // Update total price
        //        booking.Booking.TotalPrice = duration * roomOption.Price;

        //        _unitOfWork.Repository<RoomBooking>().Update(booking);
        //        await _unitOfWork.CompleteAsync();

        //        var updatedBookingDto = _mapper.Map<RoomBookingDto>(booking);
        //        return Ok(updatedBookingDto);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(new ApiResponse(401, ex.Message));
        //    }
        //}



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

       

        //// List All Bookings
        //[HttpGet("list-byAdmin")]
        //[SwaggerOperation(Summary = "Get all room bookings", Description = "Get a list of all room bookings.")]
        //public async Task<ActionResult<IReadOnlyList<RoomBookingDto>>> GetAllRoomBookings()
        //{
        //    var bookings = await _unitOfWork.Repository<RoomBooking>().GetAllAsync();
        //    var data = _mapper.Map<IReadOnlyList<RoomBooking>, IReadOnlyList<RoomBookingDto>>(bookings);

        //    return Ok(data);
        //}

        // Helper function to check if user is authenticated and fetch userId
        private async Task<string> GetAuthenticatedUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Get the user ID from the claims
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User is not authenticated");

            return userId;
        }


        // Helper method to get all dates between Checkin and Checkout
        private IEnumerable<DateTime> GetDatesInRange(DateTime checkin, DateTime checkout)
        {
            for (var date = checkin.Date; date <= checkout.Date; date = date.AddDays(1))
            {
                yield return date;
            }
        }
    }
}
