using System.ComponentModel.DataAnnotations;

namespace go_bus_backend.Models;

public class Booking
{
    [Key]
    public int Id { get; set; }

    public int TripId { get; set; }
    public Trip.Trip Trip { get; set; }

    public int DepartureBusStopId { get; set; }
    public BusStop DepartureBusStop { get; set; }
    
    public int ArrivalBusStopId { get; set; }
    public BusStop ArrivalBusStop { get; set; }

    public int PassengerCount { get; set; }

    public List<Passenger> Passengers { get; set; }

    public BookingStatus Status { get; set; }

    public DateTime InitializationTime { get; set; }
}

public enum BookingStatus
{
    Pending,
    Confirmed,
    Cancelled
}