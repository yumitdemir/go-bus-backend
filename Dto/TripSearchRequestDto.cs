using System.Runtime.InteropServices.JavaScript;

namespace go_bus_backend.Dto;

public class TripSearchRequestDto
{
    public string tripType { get; set; }
    public int departureStopId { get; set; }
    public int arrivalStopId { get; set; }
    public string departureDate { get; set; }
    public string returnDate { get; set; }
    public int passangerCount { get; set; }
    
}