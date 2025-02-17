using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using Wego.Core.Models.Booking;
using Wego.Core.Models.Flights;
using Wego.Core.Models.Identity;
using Wego.Core.Models.Room;

namespace Wego.Repository.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Airplane> Airplanes { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Terminal> Terminals { get; set; }
        public DbSet<FlightBooking> FlightBookings { get; set; }
        public DbSet<SeatReservation> SeatReservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<RoomBookingDetails> RoomBookingDetails { get; set; }
    }
}
