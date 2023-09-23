using go_bus_backend.Data;
using go_bus_backend.Dto.BusStop;
using go_bus_backend.Dto.Driver;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace go_bus_backend.Repository;

public class BusStopRepository : IBusStopRepository
{
    private readonly DataContext _context;

    public BusStopRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ICollection<BusStop>> GetAllWithoutFilterAsync()
    {
        var busStops = await _context.BusStops.ToListAsync();
        return busStops;
    }
    
    
      public async Task<BusStopGetAllAsyncDto> GetAllAsync(string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true, int page = 1, int pageSize = 10)
    {
        var busStops = _context.BusStops.AsQueryable();
        
        // Filtering
        if (!string.IsNullOrWhiteSpace(filterQuery))
        {
            busStops = busStops.Where(x =>
                x.Name.Contains(filterQuery) ||
                x.Id.ToString() == filterQuery ||
                x.Address.Contains(filterQuery) ||
                x.City.Contains(filterQuery) ||
                x.Country.Contains(filterQuery) ||
                x.Name.Contains(filterQuery) 
            );
        }

        // Sorting
        if (string.IsNullOrWhiteSpace(sortBy) == false)
        {
            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                busStops = isAscending ? busStops.OrderBy(x => x.Name) : busStops.OrderByDescending(x => x.Name);
            } else if (sortBy.Equals("Id", StringComparison.OrdinalIgnoreCase))
            {
                busStops = isAscending ? busStops.OrderBy(x => x.Id) : busStops.OrderByDescending(x => x.Id);
            } else if (sortBy.Equals("Address", StringComparison.OrdinalIgnoreCase))
            {
                busStops = isAscending ? busStops.OrderBy(x => x.Address) : busStops.OrderByDescending(x => x.Address);
            }else if (sortBy.Equals("City", StringComparison.OrdinalIgnoreCase))
            {
                busStops = isAscending ? busStops.OrderBy(x => x.City) : busStops.OrderByDescending(x => x.City);
            }
            else if (sortBy.Equals("Country", StringComparison.OrdinalIgnoreCase))
            {
                busStops = isAscending ? busStops.OrderBy(x => x.Country) : busStops.OrderByDescending(x => x.Country);
            } 
        }

        // Calculate the total count of buses (before pagination)
        int totalCount = await busStops.CountAsync();

        // Pagination
        var skipResults = (page - 1) * pageSize;
        List<BusStop> pagedBusStops = await busStops.Skip(skipResults).Take(pageSize).ToListAsync();

        // Calculate the biggest page number
        var biggestPageNumber = (int)Math.Ceiling((double)totalCount / pageSize);
        var getAllAsyncDto = new BusStopGetAllAsyncDto()
        {
            BusStops = pagedBusStops,
            BiggestPageNumber = biggestPageNumber
        };

        return getAllAsyncDto;
    }

    public async Task<BusStop?> GetById(int id)
    {
        var busStop = await _context.BusStops.FirstOrDefaultAsync(x => x.Id == id);
        return busStop;
    }

    public async Task<BusStop> CreateAsync(BusStop busStop)
    {
        await _context.BusStops.AddAsync(busStop);
        await _context.SaveChangesAsync();
        return busStop;
    }
    public async Task<bool> IsBusStopInUse(int id)
    {
        var trips = await _context.Trips.Include(x => x.Route).ThenInclude(x => x.RouteSegments).ToListAsync();
        
        if (trips.Any(t => t.Route.RouteSegments.Any(rs => rs.ArrivalStopId == id)) || trips.Any(t => t.Route.RouteSegments.Any(rs => rs.DepartureStopId == id)) )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public async Task<BusStop?> UpdateAsync(int id, BusStop busStop)
    {
        var existingBusStop = await _context.BusStops.FirstOrDefaultAsync(x => x.Id == id);
        if (existingBusStop == null)
        {
            return null;
        }

        existingBusStop.Name = busStop.Name;
        existingBusStop.City = busStop.City;
        existingBusStop.Address = busStop.Address;
        existingBusStop.Country = busStop.Country;
        


        await _context.SaveChangesAsync();
        return existingBusStop;
    }

    public async Task<BusStop?> DeleteAsync(int id)
    {
        var existingBusStop = await _context.BusStops.FirstOrDefaultAsync(x => x.Id == id);
        if (existingBusStop == null)
        {
            return null;
        }

        _context.Remove(existingBusStop);
        await _context.SaveChangesAsync();
        return existingBusStop;
    }
}