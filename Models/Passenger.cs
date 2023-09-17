using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using go_bus_backend.Models;

public class Passenger
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public int SeatNumber { get; set; }


}