using go_bus_backend.Models;

namespace go_bus_backend.Dto;

public class TripSearchAnswerDto
{
    public int TripId { get; set; }

    public Models.BusStop  ArrivalStop{ get; set; }
    public Models.BusStop  DepartureStop{ get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public Bus Bus { get; set; }
    public decimal Price { get; set; }
}