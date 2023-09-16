using go_bus_backend.Data;
using go_bus_backend.Dto;
using go_bus_backend.Dto.Route;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using go_bus_backend.Models.Trip;
using Microsoft.EntityFrameworkCore;

namespace go_bus_backend.Repository;

public class TripRepository : ITripRepository
{
    private readonly DataContext _context;

    public TripRepository(DataContext context)
    {
        _context = context;
    }
    
     public async Task<TripGetAllAsyncDto> GetAllAsync(string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true, int page = 1, int pageSize = 10)
    {
        var trips = _context.Trips.Include(r => r.Route)
            .Include(r => r.Bus).Include(r => r.TripSegments).AsQueryable();

        // Filtering
        if (!string.IsNullOrWhiteSpace(filterQuery))
        {
            trips = trips.Where(x =>
                x.DepartureDate.ToString().Contains(filterQuery) ||
                x.Id.ToString() == filterQuery ||
                x.Route.RouteName.Contains(filterQuery) 
            );
        }

        // Sorting
        if (string.IsNullOrWhiteSpace(sortBy) == false)
        {
            if (sortBy.Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                trips  = isAscending ? trips.OrderBy(x => x.Id) : trips.OrderByDescending(x => x.Id);
            }
          
        }

        // Calculate the total count of buses (before pagination)
        int totalCount = await trips.CountAsync();

        // Pagination
        var skipResults = (page - 1) * pageSize;
        List<Trip> pagedTrips = await trips.Skip(skipResults).Take(pageSize).ToListAsync();

        // Calculate the biggest page number
        var biggestPageNumber = (int)Math.Ceiling((double)totalCount / pageSize);
        var getAllAsyncDto = new TripGetAllAsyncDto()
        {
            Trips = pagedTrips,
            BiggestPageNumber = biggestPageNumber
        };

        return getAllAsyncDto;
    }

    public async Task<Trip?> GetById(int id)
    {
        var trip = await _context.Trips.Include(r => r.Route)
            .Include(r => r.Bus).Include(r => r.TripSegments).FirstOrDefaultAsync(x => x.Id == id);

        return trip;
    }

    public async Task<Trip> CreateAsync(Trip trip)
    {
        await _context.Trips.AddAsync(trip);
        await _context.SaveChangesAsync();
        return trip;
    }

    public async Task<Trip?> UpdateAsync(int id, Trip trip)
    {
        var existingTrip = await _context.Trips.FirstOrDefaultAsync(x => x.Id == id);
        if (existingTrip == null)
        {
            return null;
        }

        existingTrip.TripSegments = trip.TripSegments;
        existingTrip.Route = trip.Route;
        existingTrip.DepartureDate = trip.DepartureDate;
        existingTrip.PricePerKm = trip.PricePerKm;
        existingTrip.Bus = trip.Bus;


        await _context.SaveChangesAsync();
        return existingTrip;
    }

    public async Task<Trip?> DeleteAsync(int id)
    {
        var existingTrip = await _context.Trips.FirstOrDefaultAsync(x => x.Id == id);
        if (existingTrip == null)
        {
            return null;
        }
        _context.RouteSegments.RemoveRange(existingTrip.Route.RouteSegments);
        _context.Routes.RemoveRange(existingTrip.Route);
        _context.Remove(existingTrip);
        await _context.SaveChangesAsync();
        return existingTrip;
    }
    
    public async Task<TripSegment?> CreateTripSegmentAsync(TripSegment tripSegment)
    {
        await _context.TripSegments.AddAsync(tripSegment);
        await _context.SaveChangesAsync();
        return tripSegment;
    }
}