using go_bus_backend.Models;

namespace go_bus_backend.Dto.BusStop;

public class UpdateBusStopRequestDto
{
    public string Name { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Address { get; set; }
}