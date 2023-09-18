namespace go_bus_backend.Dto;

public class TripSearchAnswerDto
{
    public int tripId { get; set; }

    public Models.BusStop  arrivalStop{ get; set; }
    public Models.BusStop  departureStop{ get; set; }
    public TimeSpan duration { get; set; }
    public DateTime departureTime { get; set; }
    public DateTime arrivalTime { get; set; }
}