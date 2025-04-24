using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Hotels;

namespace Wego.Core.Specifications.BookingSpacification
{
    public class AvailableRoomsSpecification : BaseSpecifcation<Room>
    {
        public AvailableRoomsSpecification(RoomAvailabilitySpecParams p)
            : base(r =>
                // ينتمي للفندق المطلوب
                r.HotelId == p.HotelId

                // عنده خيار (Option) يستوعب العدد المطلوب
                && r.RoomOptions.Any(ro => ro.Capacity >= p.Guests + p.Children)

                // ومافيش عليه حجز يتداخل مع المدة المطلوبة
                && !r.RoomBookings.Any(rb =>
                    p.Checkin < rb.Checkout && p.Checkout > rb.Checkin
                )
            )
        {
            // تجمّع البيانات اللي ممكن تحتاجها في الـ DTO
            Includes.Add(r => r.RoomOptions);
            Includes.Add(r => r.Images);
            Includes.Add(r => r.Amenities);
            // لو حابب ترجع بيانات إضافية:
            // Includes.Add(r => r.Hotel);

            // ترتيب اختياري حسب الباراميتر Sort
            if (!string.IsNullOrEmpty(p.Sort))
            {
                switch (p.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(r => r.RoomOptions.Min(ro => ro.Price));
                        break;
                    case "priceDesc":
                        AddOrderByDesc(r => r.RoomOptions.Max(ro => ro.Price));
                        break;
                    case "capacityAsc":
                        AddOrderBy(r => r.RoomOptions.Min(ro => ro.Capacity));
                        break;
                    case "capacityDesc":
                        AddOrderByDesc(r => r.RoomOptions.Max(ro => ro.Capacity));
                        break;
                    default:
                        // لا ترتيب افتراضي
                        break;
                }
            }

            // تطبيق الترقيم (Pagination)
            ApplyPagination((p.PageIndex - 1) * p.PageSize, p.PageSize);
        }
    }

}
