using go_bus_backend.Dto;
using go_bus_backend.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace go_bus_backend.Controllers;
[Route("api/[controller]")]
public class TripSearchController : ControllerBase
{
    private readonly ITripRepository _tripRepository;

    public TripSearchController(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> Create([FromQuery] TripSearchRequestDto tripSearchRequestDto)
    {
        var trips = await _tripRepository.GetAllTrips(tripSearchRequestDto.departureStopId,tripSearchRequestDto.arrivalStopId,DateOnly.Parse(tripSearchRequestDto.departureDate),tripSearchRequestDto.passangerCount);
        return Ok(trips);
    }
 

}