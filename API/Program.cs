using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                var dbContext = services.GetRequiredService<ApplicationDbContext>();
                await dbContext.Database.MigrateAsync(); 

                ////await WegoContextSeed.SeedAsync(dbContext);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating or seeding the database.");
            }


            // 🔹 استخدام Exception Middleware المخصص
            app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILoggerFactory>());

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowAllOrigins");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();


            #region Demo 
            //var builder = WebApplication.CreateBuilder(args);
            //var configuration = builder.Configuration;

            //#region Database Configuration
            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseLazyLoadingProxies()
            //           .UseSqlServer(configuration.GetConnectionString("wego1"))
            //);
            //#endregion

            //#region Identity Configuration
            //builder.Services.AddIdentity<AppUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();
            //#endregion

            //#region Dependency Injection (Services & Repositories)
            //builder.Services.AddScoped<IAuthService, AuthService>();

            //builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            //builder.Services.AddScoped<IEmailService, EmailService>();
            //builder.Services.AddAutoMapper(typeof(MappingProfile));
            //#endregion

            //#region JWT Authentication
            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateAudience = true,
            //        ValidAudience = configuration["JWT:ValidAudience"],
            //        ValidateIssuer = true,
            //        ValidIssuer = configuration["JWT:ValidIssuer"],
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
            //        ValidateLifetime = true,
            //        ClockSkew = TimeSpan.FromMinutes(20) 
            //    };
            //});
            //#endregion

            //#region CORS Configuration
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAllOrigins", policy =>
            //    {
            //        policy.AllowAnyOrigin()
            //              .AllowAnyMethod()
            //              .AllowAnyHeader();
            //    });
            //});

            //#endregion

            //#region Controllers Configuration
            //builder.Services.AddControllers()
            //    .AddJsonOptions(options =>
            //    {
            //        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            //        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            //    });
            //#endregion

            //#region Swagger Configuration
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Title = "Booking API",
            //        Version = "v1",
            //        Description = "This is a booking API developed by Ahmed Mahmoud Abo Fandoud.",
            //        Contact = new OpenApiContact
            //        {
            //            Name = "Ahmed Mahmoud Abo Fandoud",
            //            Email = "ahmedfandoud077@gmail.com",
            //            Url = new Uri("https://www.linkedin.com/in/ahmed-fandoud-a334b7230")
            //        }
            //    });

            //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.Http,
            //        Scheme = "Bearer",
            //        BearerFormat = "JWT",
            //        In = ParameterLocation.Header,
            //        Description = "Enter 'Bearer' [space] and then your valid JWT token."
            //    });

            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //    {
            //        {
            //            new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference
            //                {
            //                    Type = ReferenceType.SecurityScheme,
            //                    Id = "Bearer"
            //                }
            //            },
            //            new string[] {}
            //        }
            //    });
            //});
            //#endregion

            //var app = builder.Build();
            //#region Update Database
            //// Using => to Make Dispose()
            //using var scope = app.Services.CreateScope();

            //var services = scope.ServiceProvider;

            //var loggerFactory = services.GetRequiredService<ILoggerFactory>(); // make error shown in Kestrel Screen

            //try
            //{
            //    var dbContext = services.GetRequiredService<ِApplicationDbContext>(); // Ask Explicity CLR to Make Object from StoreContext

            //    await dbContext.Database.MigrateAsync(); // Apply Migration

            //    // Calling Seeds
            //    await StoreContextSeed.SeedAsync(dbContext);

            //    // Update DataBase for IdentityUser
            //    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
            //    await identityContext.Database.MigrateAsync();

            //    // Call Seeding of UserIdentity
            //    var userManager = services.GetRequiredService<UserManager<AppUser>>();
            //    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);

            //    // Update Migration for OrderModule


            //}
            //catch (Exception ex)
            //{
            //    var logger = loggerFactory.CreateLogger<Program>();
            //    logger.LogError(ex, " An Error Occured During Applying Migrations !");
            //}

            //#endregion
            //#region Middleware Configuration
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            //app.UseExceptionHandler("/error"); // ✅ أفضل لمعالجة الأخطاء
            //app.UseHttpsRedirection();
            //app.UseStaticFiles();
            //app.UseCors("AllowAllOrigins");

            //app.UseRouting();
            //app.UseAuthentication();
            //app.UseAuthorization();

            //app.MapControllers();
            //#endregion

            //app.Run(); 
            #endregion
        }
    }
}
