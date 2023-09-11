namespace go_bus_backend.Dto.Driver;

public class AddDriverRequestDto
{
    public string Name { get; set; }
    public string Surname { get; set; }

    public string LicenseNumber { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public string ContactNumber { get; set; }

    public string DriverStatus { get; set; }
}