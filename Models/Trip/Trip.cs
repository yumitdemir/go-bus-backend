namespace go_bus_backend.Models.Trip;

public class Trip
{
    public int Id { get; set; }
    public Route Route { get; set; }
    public ICollection<TripSegment> TripSegments { get; set; }
    public decimal PricePerKm { get; set; }
    public Bus Bus { get; set; }
    public DateTime DepartureDate { get; set; }
}