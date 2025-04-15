using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Enums;

namespace Wego.Core.Specifications.BookingSpacification
{
    public class RoomBookingSpecParams
    {

        private const int MaxPageSize = 10;

        private int pageSize = 5;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public int PageIndex { get; set; } = 1;

        private string? search;
        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }

        public string? Sort { get; set; }

        // فلترة حسب تاريخ الوصول
        public DateTime? Checkin { get; set; }

        // فلترة حسب تاريخ المغادرة
        public DateTime? Checkout { get; set; }

        // فلترة حسب عدد الضيوف
        public int? Guests { get; set; }

        // فلترة حسب عدد الأطفال
        public int? Children { get; set; }

        // فلترة حسب حالة الحجز
        public BookingStatus? Status { get; set; }

        // فلترة حسب معرف المستخدم
        public string? UserId { get; set; }
    }
}
