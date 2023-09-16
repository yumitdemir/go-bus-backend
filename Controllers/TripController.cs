using go_bus_backend.Dto;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using go_bus_backend.Models.Trip;
using Microsoft.AspNetCore.Mvc;

namespace go_bus_backend.Controllers;

[Route("api/[controller]")]
public class TripController : ControllerBase
{
    private readonly ITripRepository _tripRepository;
    private readonly IRouteRepository _routeRepository;
    private readonly IBusRepository _busRepository;

    public TripController(ITripRepository tripRepository, IRouteRepository routeRepository,
        IBusRepository busRepository)
    {
        _busRepository = busRepository;
        _routeRepository = routeRepository;
        _tripRepository = tripRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddTripRequestDto addTripRequestDto)
    {
        var route = await _routeRepository.GetById(addTripRequestDto.RouteId);
        var bus = await _busRepository.GetById(addTripRequestDto.BusId);
        var tripSegments = new List<TripSegment>();

        if (route != null)
            foreach (var routeSegment in route.RouteSegments)
            {
                var tripSegment = new TripSegment()
                {
                    RouteSegment = routeSegment,
                };

                var createdTripSegment = await _tripRepository.CreateTripSegmentAsync(tripSegment);
                if (createdTripSegment != null) tripSegments.Add(createdTripSegment);
            }


        var trip = new Trip()
        {
            Route = route,
            Bus = bus,
            DepartureDate = addTripRequestDto.DepartureDate,
            PricePerKm = addTripRequestDto.PricePerKm,
            TripSegments = tripSegments
        };
        
        var createdDriver = await _tripRepository.CreateAsync(trip);
        return Ok(trip);
    }

    [HttpPut]
    public async Task<IActionResult?> Update(int id, [FromBody] UpdateTripRequestDto updateDriverRequestDto)
    {
        var route = await _routeRepository.GetById(updateDriverRequestDto.RouteId);
        var bus = await _busRepository.GetById(updateDriverRequestDto.BusId);
        var tripSegments = new List<TripSegment>();

        if (route != null)
            foreach (var routeSegment in route.RouteSegments)
            {
                var tripSegment = new TripSegment()
                {
                    RouteSegment = routeSegment,
                };

                var createdTripSegment = await _tripRepository.CreateTripSegmentAsync(tripSegment);
                if (createdTripSegment != null) tripSegments.Add(createdTripSegment);
            }


        var trip = new Trip()
        {
            Route = route,
            Bus = bus,
            DepartureDate = updateDriverRequestDto.DepartureDate,
            PricePerKm = updateDriverRequestDto.PricePerKm,
            TripSegments = tripSegments
        };
        
        var updatedTrip = await _tripRepository.UpdateAsync(id, trip);
        return Ok(updatedTrip);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] int id)
    {
        var deletedTrip = await _tripRepository.DeleteAsync(id);
        return Ok(deletedTrip);
    }

    [HttpGet]
    public async Task<IActionResult> GetDrivers([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var trips = await _tripRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, page,
            pageSize);
        return Ok(trips);
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(int id)
    {
        var bus = await _tripRepository.GetById(id);
        return Ok(bus);
    }
}