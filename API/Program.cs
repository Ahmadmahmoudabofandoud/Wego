using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using System.Text;
using Wego.Core;
using Wego.Core.Models.Identity;
using Wego.Core.Services.Contract;
using Wego.Repository;
using Wego.Repository.Data;
using Wego.Service;

namespace Wego.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
       

            // Context
            builder.Services.AddDbContext<ApplicationDbContext>(op =>
                op.UseLazyLoadingProxies()
                .UseSqlServer(builder.Configuration.GetConnectionString("wego1"))
            );

            builder.Services.AddIdentity<AppUser, IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               // Configure Authentication Handler
               options.TokenValidationParameters = new TokenValidationParameters()
               {
                   ValidateAudience = true,
                   ValidAudience = configuration["JWT:ValidAudience"],
                   ValidateIssuer = true,
                   ValidIssuer = configuration["JWT:ValidIssuer"],
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
                   ValidateLifetime = true,
                   ClockSkew = TimeSpan.FromDays(double.Parse(configuration["JWT:DurationInDays"]))

               };
           });
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        

            // Add DI services.
            #region Service/Repor (DI)
            // Service / unit
            //builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            //builder.Services.AddScoped<IAirlineService, AirlineService>();
            //builder.Services.AddScoped<IAirplaneService, AirplaneService>();
            //builder.Services.AddScoped<IPaymentService, PaymentService>();
            //builder.Services.AddScoped<IFlightBookingService, FlightBookingService>();
            //builder.Services.AddScoped<IRoomRepository, RoomsRepository>();
            //builder.Services.AddScoped<IBookingRepositroy, BookingRepositroy>();
            //builder.Services.AddScoped<IRoomDetailsRepository, RoomDetailsRepository>();
            //builder.Services.AddSingleton<IEmailService, EmailService>();
            //builder.Services.AddSingleton<IMapper, Mapper.Mapper>();
            //builder.Services.AddAutoMapper(typeof(Program));
            #endregion

            #region Stripe
            //var stripeSettings = builder.Configuration.GetSection("Stripe");
            //StripeConfiguration.ApiKey = stripeSettings["SecretKey"];
            //builder.Services.Configure<StripeSettings>(stripeSettings);

            #endregion


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            // Swagger
            #region SwaggerConfiguration
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Booking Api",
                    Version = "v1",
                    Description = "This is a booking API developed by Ahmed Mahmoud Abo Fandoud.",
                    Contact = new OpenApiContact
                    {
                        Name = "Ahmed Mahmoud Abo Fandoud",
                        Email = "ahmedfandoud077@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/ahmed-fandoud-a334b7230/?lipi=urn%3Ali%3Apage%3Ad_flagship3_feed%3BFbcTsJnJTN6EOHekgKR1tA%3D%3D")
                    }
                });

                // Configure Swagger to use the Authorization header for JWT
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid JWT token.",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("AllowSpecificOrigin");
            app.MapControllers();

            app.UseAuthentication();

            app.UseAuthorization();

            app.Run();
        }
    }
}
