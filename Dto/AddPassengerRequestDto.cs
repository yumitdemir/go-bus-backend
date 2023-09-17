namespace go_bus_backend.Dto;

public class AddPassengerRequestDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public int SeatNumber { get; set; }

}