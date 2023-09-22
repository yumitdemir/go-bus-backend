namespace go_bus_backend.Dto;

public class InitializeBookingRequestDto
{

    public int TripId { get; set; }

    public int DepartureBusStopId { get; set; }
    
    public int ArrivalBusStopId { get; set; }

    public int PassengerCount { get; set; }

}