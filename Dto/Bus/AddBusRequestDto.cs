namespace go_bus_backend.Dto;

public class AddBusRequestDto
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Capacity  { get; set; }
    
    public string WiFiAvailable  { get; set; }
    
    public string RestroomAvailable   { get; set; }
    
    public DateTime LastMaintenanceDate   { get; set; }
    
    public string PlateNumber  { get; set; }
}