using go_bus_backend.Models;
using go_bus_backend.Models.Trip;
using Microsoft.EntityFrameworkCore;
using Route = go_bus_backend.Models.Route;

namespace go_bus_backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Bus> Buses { get; set; }
        public DbSet<BusStop> BusStops { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<RouteSegment> RouteSegments { get; set; }
        public DbSet<TripSegment> TripSegments { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<RouteSegment>()
                .HasOne(rs => rs.DepartureStop)
                .WithMany()
                .HasForeignKey(rs => rs.DepartureStopId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RouteSegment>()
                .HasOne(rs => rs.ArrivalStop)
                .WithMany()
                .HasForeignKey(rs => rs.ArrivalStopId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TripSegment>()
                .HasOne(ts => ts.RouteSegment)
                .WithMany()
                .HasForeignKey(ts => ts.RouteSegmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(ts => ts.DepartureBusStop)
                .WithMany()
                .HasForeignKey(ts => ts.DepartureBusStopId)
                .OnDelete(DeleteBehavior.Restrict); // Change here

            modelBuilder.Entity<Booking>()
                .HasOne(ts => ts.ArrivalBusStop)
                .WithMany()
                .HasForeignKey(ts => ts.ArrivalBusStopId)
                .OnDelete(DeleteBehavior.Restrict); // Change here

            modelBuilder.Entity<Booking>()
                .HasOne(ts => ts.Trip)
                .WithMany()
                .HasForeignKey(ts => ts.TripId)
                .OnDelete(DeleteBehavior.Restrict); // Change here


            base.OnModelCreating(modelBuilder);
        }
    }
}