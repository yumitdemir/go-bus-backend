using go_bus_backend.Models;
using go_bus_backend.Models.Trip;
using Microsoft.EntityFrameworkCore;

namespace go_bus_backend.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    public DbSet<Bus> Buses { get; set; }
    public DbSet<BusStop> BusStops { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<TripStop> TripStops { get; set; }
    public DbSet<Trip> Trips { get; set; }

}