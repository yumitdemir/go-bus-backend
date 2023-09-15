using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace go_bus_backend.Models;

public class RouteSegment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Foreign keys
    public int DepartureStopId { get; set; }
    public int ArrivalStopId { get; set; }

    // Navigation properties
    public BusStop DepartureStop { get; set; }
    public BusStop ArrivalStop { get; set; }

    public TimeSpan Duration { get; set; }
    public double Distance { get; set; }
    
}