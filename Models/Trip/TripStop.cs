using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace go_bus_backend.Models.Trip;

public class TripStop
{
    [Key]
    public int Id { get; set; }
    public virtual BusStop BusStop { get; set; }
    public DateTime ArrivalTime { get; set; }
    public double Distance { get; set; }
    public int PassengerCount { get; set; }
    
    [ForeignKey("Trip")] 
    public int TripId { get; set; }
    public Trip Trip { get; set; }
}