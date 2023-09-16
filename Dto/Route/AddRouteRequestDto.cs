namespace go_bus_backend.Dto.Route;

public class AddRouteRequestDto
{
     public string RouteName { get; set; }
    public ICollection<int> RouteSegmentIds { get; set; }
    public ICollection<int> BusStopIds { get; set; }
}