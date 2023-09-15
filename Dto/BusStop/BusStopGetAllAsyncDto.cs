namespace go_bus_backend.Dto.BusStop;

public class BusStopGetAllAsyncDto
{
    public ICollection<Models.BusStop>? BusStops { get; set; }
    public int BiggestPageNumber { get; set; }
}