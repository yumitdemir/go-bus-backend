namespace go_bus_backend.Dto.Route;

public class UpdateRouteRequestDto
{
    public string RouteName { get; set; }
    public ICollection<int> RouteSegmentIds { get; set; }
}