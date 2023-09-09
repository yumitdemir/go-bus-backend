namespace go_bus_backend.Models.Trip;

public class Trip
{
    public int Id { get; set; }
    public ICollection<TripStop> TripStops { get; set; }
    public Bus Bus { get; set; }
    public decimal PricePerKm { get; set; }
}