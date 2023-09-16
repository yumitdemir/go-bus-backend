namespace go_bus_backend.Dto.Route;

public class UpdateRouteRequestDto
{
    public string RouteName { get; set; }
    public ICollection<Models.RouteSegment> RouteSegments { get; set; }
    public  ICollection<Models.BusStop> BusStops { get; set; }
}