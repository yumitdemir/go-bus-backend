using go_bus_backend.Models.Trip;

namespace go_bus_backend.Dto;

public class TripGetAllAsyncDto
{
    public ICollection<Trip> Trips { get; set; }
    public int BiggestPageNumber { get; set; }
}