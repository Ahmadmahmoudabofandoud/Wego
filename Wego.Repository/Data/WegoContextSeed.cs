using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wego.Core.Models.Flights;
using Wego.Core.Models.Hotels;
using Wego.Core.Models;

namespace Wego.Repository.Data
{
    public static class WegoContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            #region Airlines Seeding
            if (!dbContext.Airlines.Any())
            {
                var airlinesData = File.ReadAllText("../Wego.Repository/Data/DataSeed/AirlinesDataSeed.json");
                var airlines = JsonSerializer.Deserialize<List<Airline>>(airlinesData);

                if (airlines is not null && airlines.Count > 0)
                {
                    await dbContext.Set<Airline>().AddRangeAsync(airlines);
                    await dbContext.SaveChangesAsync();
                }
            }
            #endregion

            #region Hotels Seeding
            if (!dbContext.Hotels.Any())
            {
                var hotelsData = File.ReadAllText("../Wego.Repository/Data/DataSeed/HotelsDataSeed.json");
                var hotels = JsonSerializer.Deserialize<List<Hotel>>(hotelsData);

                if (hotels is not null && hotels.Count > 0)
                {
                    await dbContext.Set<Hotel>().AddRangeAsync(hotels);
                    await dbContext.SaveChangesAsync();
                }
            }
            #endregion

            #region Locations Seeding
            if (!dbContext.Locations.Any())
            {
                var locationsData = File.ReadAllText("../Wego.Repository/Data/DataSeed/LocationsDataSeed.json");
                var locations = JsonSerializer.Deserialize<List<Location>>(locationsData);

                if (locations is not null && locations.Count > 0)
                {
                    await dbContext.Set<Location>().AddRangeAsync(locations);
                    await dbContext.SaveChangesAsync();
                }
            }
            #endregion
        }
    }
}
