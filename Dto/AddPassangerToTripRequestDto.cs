using go_bus_backend.Models;

namespace go_bus_backend.Dto;

public class AddPassangerToTripRequestDto
{
    public int TripId { get; set; }
    public int DepartureBusStopId { get; set; }
    public int ArrivalBusStopId { get; set; }
    public Passenger Passanger { get; set; }
}