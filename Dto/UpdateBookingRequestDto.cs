namespace go_bus_backend.Dto;

public class UpdateBookingRequestDto
{
    public int BookingId { get; set; }
    public List<Passenger> Passengers { get; set; }
}