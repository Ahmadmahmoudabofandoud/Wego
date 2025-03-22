using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Wego.Core.Models.Identity;
using Wego.Repository.Data;

namespace Wego.API.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
