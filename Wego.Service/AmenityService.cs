using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models.Hotels;
using Wego.Repository.Data;

namespace Wego.Service
{
    public class AmenityService
    {
        private readonly ApplicationDbContext _context;

        public AmenityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Amenity>> GetAmenitiesByIdsAsync(List<int> amenityIds)
        {
            return await _context.Amenities
                .Where(a => amenityIds.Contains(a.Id))
                .ToListAsync();  // تأكد من استخدام ToListAsync بدلاً من ToList
        }
    }

}
