using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Enums;

namespace Wego.Core.Specifications.RoomSpecification
{
    public class RoomSpecParams
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

        public string? Sort { get; set; }

        // فلترة حسب السعر
        public int? Price { get; set; }

        // فلترة حسب التقييم
        public int? Rating { get; set; }

        // فلترة حسب نوع الغرفة
        public RoomType? RoomType { get; set; }

        // فلترة حسب حالة الغرفة (نشطة أم لا)
        public bool? IsActive { get; set; }
    }

}
