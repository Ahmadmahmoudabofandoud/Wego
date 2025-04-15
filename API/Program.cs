using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Wego.API.Extensions;
using Wego.API.Middlewares;
using Wego.Core;
using Wego.Core.Models.Identity;
using Wego.Core.Repositories.Contract;
using Wego.Core.Services.Contract;
using Wego.Repository;
using Wego.Repository.Data;
using Wego.Service;

namespace Wego.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            #region Dependancy Injection 

            builder.Services.AddDatabaseConfiguration(configuration);

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddSingleton<IEmailService, EmailService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<AmenityService>();
            builder.Services.AddScoped<LocationService>();
            builder.Services.AddIdentityConfiguration();
            builder.Services.AddJwtAuthentication(configuration);
            builder.Services.AddCorsPolicy();
            builder.Services.AddSwaggerDocumentation(); 

            #endregion


            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<ApplicationDbContext>();
            var logger = services.GetRequiredService<ILogger<ILoggerFactory>>();


            try
            {
                await dbContext.Database.MigrateAsync();
                //await WegoContextSeed.SeedAsync(dbContext);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating or seeding the database.");
            }

            app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILoggerFactory>());
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wego API V1");
                c.RoutePrefix = "swagger";
            });


            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowAllOrigins");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();


        }
    }
}
