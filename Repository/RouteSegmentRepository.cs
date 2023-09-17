using go_bus_backend.Data;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using Microsoft.EntityFrameworkCore;
using Route = Microsoft.AspNetCore.Routing.Route;

namespace go_bus_backend.Repository;

public class RouteSegmentRepository : IRouteSegmentRepository
{
    private readonly DataContext _context;

    public RouteSegmentRepository(DataContext context)
    {
        _context = context;
    }

    
    public async Task<RouteSegment?> GetByIdAsync(int id)
    {
        var routeSegment = await _context.RouteSegments.Include(r=> r.ArrivalStop).Include(r=> r.DepartureStop).FirstOrDefaultAsync(x => x.Id == id);
        return routeSegment;
    }

    public async Task<RouteSegment> CreateAsync(RouteSegment routeSegment)
    {
        await _context.RouteSegments.AddAsync(routeSegment);
        await _context.SaveChangesAsync();
        return routeSegment;
    }

    public async Task<RouteSegment?> UpdateAsync(int id, RouteSegment routeSegment)
    {
        var existingRouteSegment = await _context.RouteSegments.FirstOrDefaultAsync(x => x.Id == id);
        if (existingRouteSegment == null)
        {
            return null;
        }

        existingRouteSegment.DepartureStop = routeSegment.DepartureStop;
        existingRouteSegment.ArrivalStop = routeSegment.ArrivalStop;
        existingRouteSegment.Duration = routeSegment.Duration;
        existingRouteSegment.Distance = routeSegment.Distance;
       

        await _context.SaveChangesAsync();
        return existingRouteSegment;
    }

    public async Task<RouteSegment?> DeleteAsync(int id)
    {
        var existingRouteSegments = await _context.RouteSegments.FirstOrDefaultAsync(x => x.Id == id);
        if (existingRouteSegments == null)
        {
            return null;
        }

        _context.Remove(existingRouteSegments);
        await _context.SaveChangesAsync();
        return existingRouteSegments;
    }

    
}