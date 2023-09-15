namespace go_bus_backend.Dto.Route;

public class RouteGetAllAsyncDto
{
    public ICollection<Models.Route> Routes { get; set; }
    public int BiggestPageNumber { get; set; }
}