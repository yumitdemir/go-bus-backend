namespace go_bus_backend.Dto;

public class AddTripRequestDto
{
    public int RouteId { get; set; }
    public decimal PricePerKm { get; set; }
    public int BusId { get; set; }
    public DateTime DepartureDate { get; set; }

    public ICollection<string> TimeOfDay { get; set; }
    public ICollection<string> DayOfWeek { get; set; }
    public ICollection<string> UnavailableDates { get; set; }
    public string LastAvailableDate { get; set; }
    public string StartDate { get; set; }
    
}