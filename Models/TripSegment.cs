namespace go_bus_backend.Models;

public class TripSegment
{
    public int Id { get; set; }
    public RouteSegment RouteSegment { get; set; }
    public ICollection<Passanger>? Passengers { get; set; }
}