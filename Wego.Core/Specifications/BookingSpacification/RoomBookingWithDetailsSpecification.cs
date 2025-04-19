using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Booking;

namespace Wego.Core.Specifications.BookingSpacification
{
    public class RoomBookingWithDetailsSpecification : BaseSpecifcation<RoomBooking>
    {
        public RoomBookingWithDetailsSpecification(RoomBookingSpecParams specParams)
                : base(RB =>
                    (string.IsNullOrEmpty(specParams.Search) ||
                     (RB.Room.RoomTitle != null && RB.Room.RoomTitle.ToLower().Contains(specParams.Search)) ||
                     (RB.Booking.UserId != null && RB.Booking.UserId.ToLower().Contains(specParams.Search))) &&
                    (!specParams.Checkin.HasValue || RB.Checkin.Date < specParams.Checkout.Value.Date) &&
                    (!specParams.Checkout.HasValue || RB.Checkout.Date > specParams.Checkin.Value.Date) &&
                    (!specParams.Guests.HasValue || RB.Guests == specParams.Guests) &&
                    (!specParams.Children.HasValue || RB.Children == specParams.Children) &&
                    (!specParams.Status.HasValue || RB.Booking.Status == specParams.Status) &&
                    (specParams.RoomIds == null || specParams.RoomIds.Contains(RB.RoomId))
                )

        {
            AddIncludes();

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "checkinDesc":
                        AddOrderByDesc(RB => RB.Checkin);
                        break;
                    case "checkinAsc":
                        AddOrderBy(RB => RB.Checkin);
                        break;
                    case "checkoutDesc":
                        AddOrderByDesc(RB => RB.Checkout);
                        break;
                    case "checkoutAsc":
                        AddOrderBy(RB => RB.Checkout);
                        break;
                }
            }

            // Apply pagination
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }

        public RoomBookingWithDetailsSpecification(int id)
            : base(RB => RB.Id == id)
        {

            AddIncludes();

        }

        private void AddIncludes()
        {
            Includes.Add(RB => RB.Room);      // Includes Room details
            Includes.Add(RB => RB.Booking);   // Includes HotelBooking details
        }
    }
}
