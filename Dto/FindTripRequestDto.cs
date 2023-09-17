using go_bus_backend.Models;

namespace go_bus_backend.Dto;

public class FindTripRequestDto
{
    public int DepartureBusStopId { get; set; }
    public int ArrivalBusStopId { get; set; }
}