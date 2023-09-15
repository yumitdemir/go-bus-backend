namespace go_bus_backend.Dto.RouteSegment;

public class AddRouteSegmentRequestDto
{
    public int DepartureStopId { get; set; }
    public int ArrivalStopId { get; set; }
    public string Duration { get; set; }
    public double Distance { get; set; }
}