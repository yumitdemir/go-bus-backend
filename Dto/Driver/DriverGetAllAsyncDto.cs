namespace go_bus_backend.Dto.Driver;

public class DriverGetAllAsyncDto
{
    public ICollection<Models.Driver>? Drivers { get; set; }
    public int BiggestPageNumber { get; set; }
}