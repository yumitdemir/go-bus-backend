using System.ComponentModel.DataAnnotations;

namespace go_bus_backend.Models;

public class BusStop
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; } 
}