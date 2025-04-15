using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Enums;

namespace Wego.Core.Specifications.AirlineSpecification
{
    public class FlightSpecParams
    {
        private const int MaxPageSize = 10;
        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        private string? search;
        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }

        public int PageIndex { get; set; } = 1;

        // ترتيب الرحلات حسب السعر، الوقت، أو أي معيار آخر
        public string? Sort { get; set; }

        // تصفية الرحلات حسب شركة الطيران
        public int? AirlineId { get; set; }

        // تصفية الرحلات حسب الطائرة
        public int? AirplaneId { get; set; }

        // تصفية الرحلات حسب مطار المغادرة
        public int? DepartureAirportId { get; set; }

        // تصفية الرحلات حسب مطار الوصول
        public int? ArrivalAirportId { get; set; }

        // تصفية الرحلات حسب تاريخ المغادرة
        public DateTime? DepartureDate { get; set; }

        // تصفية الرحلات حسب الحد الأدنى للسعر
        public double? MinPrice { get; set; }

        // تصفية الرحلات حسب الحد الأقصى للسعر
        public double? MaxPrice { get; set; }

        // تصفية الرحلات حسب حالة الرحلة (Scheduled, Delayed, Canceled, etc.)
        public FlightStatus? Status { get; set; }
    }

}
