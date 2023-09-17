using System.Text.Json;
using System.Text.Json.Serialization;
using go_bus_backend.Dto;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace go_bus_backend.Controllers;

[Route("api/[controller]")]
public class PassengerController : ControllerBase
{
    private readonly IPassangerRepository _passangerRepository;
    private readonly ITripRepository _tripRepository;

    public PassengerController(IPassangerRepository passangerRepository, ITripRepository tripRepository)
    {
        _passangerRepository = passangerRepository;
        _tripRepository = tripRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddPassengerRequestDto addPassengerRequestDto)
    {

        var newPassenger = new Passenger()
        {
            Email = addPassengerRequestDto.Email,
            Name = addPassengerRequestDto.Name,
            Surname = addPassengerRequestDto.Surname,
            PhoneNumber = addPassengerRequestDto.PhoneNumber,
            SeatNumber = addPassengerRequestDto.SeatNumber,
        };

        var createdPassenger = await _passangerRepository.CreateAsync(newPassenger);
        return Ok(createdPassenger);
    }
}