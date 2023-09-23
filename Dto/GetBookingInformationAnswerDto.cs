using go_bus_backend.Models;

namespace go_bus_backend.Dto;

public class GetBookingInformationAnswerDto
{
    public Booking Booking { get; set; }
    public DateTime TripStart { get; set; }
    public DateTime TripEnd { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string BookingPassengerName { get; set; }
}