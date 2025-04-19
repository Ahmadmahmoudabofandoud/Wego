using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Booking;

namespace Wego.Core.Specifications.BookingSpacification
{
    public class RoomBookingWithFilterationForCountSpecifications : BaseSpecifcation<RoomBooking>
    {
        public RoomBookingWithFilterationForCountSpecifications(RoomBookingSpecParams specParams)
            : base(RB =>
                    (string.IsNullOrEmpty(specParams.Search) ||
                     (RB.Room != null && RB.Room.RoomTitle.ToLower().Contains(specParams.Search)) ||
                     (RB.Booking != null && RB.Booking.UserId.ToLower().Contains(specParams.Search)) ||
                     (RB.Booking != null && RB.Booking.Status.ToString().ToLower().Contains(specParams.Search))) &&
                    (!specParams.Checkin.HasValue || RB.Checkin >= specParams.Checkin) &&
                    (!specParams.Checkout.HasValue || RB.Checkout <= specParams.Checkout) &&
                    //(!specParams.Guests.HasValue || RB.Guests == specParams.Guests.Value) &&
                    //(!specParams.Children.HasValue || RB.Children == specParams.Children.Value) &&
                    (!specParams.Status.HasValue || RB.Booking.Status == specParams.Status.Value) &&
                    (string.IsNullOrEmpty(specParams.UserId) || RB.Booking.UserId == specParams.UserId)
        )
        {
        }
    }

}
