namespace go_bus_backend.Models;

public class Route
{
    public int Id { get; set; }
    public string RouteName { get; set; }
    public ICollection<RouteSegment> RouteSegments { get; set; }
    public  ICollection<BusStop> BusStops { get; set; }
}