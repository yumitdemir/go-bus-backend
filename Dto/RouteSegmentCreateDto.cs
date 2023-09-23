namespace go_bus_backend.Dto;

public class RouteSegmentCreateDto
{
    public int DepartureStopId { get; set; }
    public int ArrivalStopId { get; set; }
    public string Duration { get; set; }
    public double Distance { get; set; }
}