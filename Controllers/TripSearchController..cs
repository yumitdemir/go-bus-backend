using go_bus_backend.Dto;
using go_bus_backend.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace go_bus_backend.Controllers;
[Route("api/[controller]")]
public class TripSearchController : ControllerBase
{
    private readonly ITripRepository _tripRepository;
    private readonly IBusStopRepository _busStopRepository;

    public TripSearchController(ITripRepository tripRepository,IBusStopRepository busStopRepository)
    {
        _tripRepository = tripRepository;
        _busStopRepository = busStopRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] TripSearchRequestDto tripSearchRequestDto)
    {
        var trips = await _tripRepository.GetAllTrips(tripSearchRequestDto.departureStopId,tripSearchRequestDto.arrivalStopId,DateOnly.Parse(tripSearchRequestDto.departureDate),tripSearchRequestDto.passangerCount);
        var tripAnswerList = new List<TripSearchAnswerDto>();
        foreach (var trip in trips)
        {
            var tripSearchAnswerDto = new TripSearchAnswerDto()
            {
                tripId = trip.Id,
                arrivalStop = await _busStopRepository.GetById(tripSearchRequestDto.arrivalStopId),
                departureStop = await _busStopRepository.GetById(tripSearchRequestDto.departureStopId),
                duration = await _tripRepository.CalculateDurationOfTrip(tripSearchRequestDto.departureStopId,tripSearchRequestDto.arrivalStopId,trip.Id),
                arrivalTime = await _tripRepository.getStartDateTimeOfTheTrip(tripSearchRequestDto.departureStopId,trip.Id),
                departureTime = await _tripRepository.getEndDateTimeOfTheTrip(tripSearchRequestDto.arrivalStopId,tripSearchRequestDto.departureStopId,trip.Id)
                
            };
            tripAnswerList.Add(tripSearchAnswerDto);
        }
        
        
        return Ok(tripAnswerList);
    }
 

}