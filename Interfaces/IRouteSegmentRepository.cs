using go_bus_backend.Models;

namespace go_bus_backend.Interfaces;

public interface IRouteSegmentRepository
{
    public  Task<RouteSegment?> DeleteAsync(int id);
    public  Task<RouteSegment?> UpdateAsync(int id, RouteSegment routeSegment);
    public  Task<RouteSegment> CreateAsync(RouteSegment routeSegment);
    public  Task<RouteSegment?> GetByIdAsync(int id);
}