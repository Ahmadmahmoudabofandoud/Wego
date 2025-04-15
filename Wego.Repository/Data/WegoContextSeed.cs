using System.Text.Json;
using Wego.Core.Models;

namespace Wego.Repository.Data
{
    public static class WegoContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext) 
        {
            if (!dbContext.Locations.Any()) 
            {
                var LocationsData = File.ReadAllText("../Wego.Repository/Data/DataSeed/LocationsDataSeed.json"); 
                var brands = JsonSerializer.Deserialize<List<Location>>(LocationsData); 

                if (brands is not null && brands.Count > 0)
                {
                    foreach (var brand in brands)
                        await dbContext.Set<Location>().AddAsync(brand); // add brand in Db

                    await dbContext.SaveChangesAsync();
                }
            }
        }

    } 
}