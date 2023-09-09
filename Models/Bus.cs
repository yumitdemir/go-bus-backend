using System.ComponentModel.DataAnnotations;

namespace go_bus_backend.Models;

public class Bus
{
    [Key]
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Capacity  { get; set; }
    
    public bool WiFiAvailable  { get; set; }
    
    public bool RestroomAvailable   { get; set; }
    
    public DateTime LastMaintenanceDate   { get; set; }
    
    public string PlateNumber  { get; set; }
}