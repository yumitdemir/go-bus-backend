using go_bus_backend.Data;
using go_bus_backend.Dto.Driver;
using go_bus_backend.Dto.Route;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using Microsoft.EntityFrameworkCore;
using Route = go_bus_backend.Models.Route;

namespace go_bus_backend.Repository;

public class RouteRepository : IRouteRepository
{
    private readonly DataContext _context;

    public RouteRepository(DataContext context)
    {
        _context = context;
    }
  
     public async Task<RouteGetAllAsyncDto> GetAllAsync(string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true, int page = 1, int pageSize = 10)
    {
        var routes = _context.Routes.AsQueryable();
        
        // Filtering
        if (!string.IsNullOrWhiteSpace(filterQuery))
        {
            routes = routes.Where(x =>
                x.RouteName.Contains(filterQuery) ||
                x.Id.ToString() == filterQuery 
            );
        }

        // Sorting
        if (string.IsNullOrWhiteSpace(sortBy) == false)
        {
            if (sortBy.Equals("Route Name", StringComparison.OrdinalIgnoreCase))
            {
                routes = isAscending ? routes.OrderBy(x => x.RouteName) : routes.OrderByDescending(x => x.RouteName);
            } else if (sortBy.Equals("Id", StringComparison.OrdinalIgnoreCase))
            {
                routes = isAscending ? routes.OrderBy(x => x.Id) : routes.OrderByDescending(x => x.Id);
            } 
        }

        // Calculate the total count of buses (before pagination)
        int totalCount = await routes.CountAsync();

        // Pagination
        var skipResults = (page - 1) * pageSize;
        List<Route> pagedRoutes = await routes.Skip(skipResults).Take(pageSize).ToListAsync();

        // Calculate the biggest page number
        var biggestPageNumber = (int)Math.Ceiling((double)totalCount / pageSize);
        var getAllAsyncDto = new RouteGetAllAsyncDto()
        {
            Routes = pagedRoutes,
            BiggestPageNumber = biggestPageNumber
        };

        return getAllAsyncDto;
    }

    public async Task<Route?> GetById(int id)
    {
        var route = await _context.Routes.FirstOrDefaultAsync(x => x.Id == id);

        return route;
    }

    public async Task<Route> CreateAsync(Route route)
    {
        await _context.Routes.AddAsync(route);
        await _context.SaveChangesAsync();
        return route;
    }

    public async Task<Route?> UpdateAsync(int id, Route route)
    {
        var existingRoute = await _context.Routes.FirstOrDefaultAsync(x => x.Id == id);
        if (existingRoute == null)
        {
            return null;
        }

        existingRoute.RouteName = route.RouteName;
        existingRoute.RouteSegments = route.RouteSegments;
        existingRoute.BusStops = route.BusStops;
       

        await _context.SaveChangesAsync();
        return existingRoute;
    }

    public async Task<Route?> DeleteAsync(int id)
    {
        var existingRoute = await _context.Routes.FirstOrDefaultAsync(x => x.Id == id);
        if (existingRoute == null)
        {
            return null;
        }

        _context.Remove(existingRoute);
        await _context.SaveChangesAsync();
        return existingRoute;
    }

 
}