using System.ComponentModel.DataAnnotations;

namespace go_bus_backend.Models;

public class Driver
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}

