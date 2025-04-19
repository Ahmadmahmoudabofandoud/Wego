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

        public DateTime? Checkin { get; set; }

        public DateTime? Checkout { get; set; }

        public int? Guests { get; set; }

        public int? Children { get; set; }

        public BookingStatus? Status { get; set; }
        public List<int>? RoomIds { get; set; }


        public string? UserId { get; set; }
    }
}
