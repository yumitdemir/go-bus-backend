namespace go_bus_backend.Models;

public class TripSegment
{
    public int Id { get; set; }
    public RouteSegment RouteSegment { get; set; }
    public int PassengerCount { get; set; }
}