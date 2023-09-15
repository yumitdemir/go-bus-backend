using go_bus_backend.Models;
using go_bus_backend.Models.Trip;
using Microsoft.EntityFrameworkCore;
using Route = go_bus_backend.Models.Route;

namespace go_bus_backend.Data;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<RouteSegment>()
            .HasOne(p => p.ArrivalStop)
            .WithMany()
            .HasForeignKey(p => p.ArrivalStopId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<RouteSegment>()
            .HasOne(p => p.DepartureStop)
            .WithMany()
            .HasForeignKey(p => p.DepartureStopId)
            .OnDelete(DeleteBehavior.NoAction);
        
      
    }


}