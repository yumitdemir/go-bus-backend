namespace go_bus_backend.Dto;

public class AddTripRequestDto
{
    public int RouteId { get; set; }
    public decimal PricePerKm { get; set; }
    public int BusId { get; set; }
    public DateTime DepartureDate { get; set; }
}