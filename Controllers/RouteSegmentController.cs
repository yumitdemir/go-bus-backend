using go_bus_backend.Dto.RouteSegment;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace go_bus_backend.Controllers;

[Route("api/[controller]")]
public class RouteSegmentController : ControllerBase
{
    private readonly IRouteSegmentRepository _routeSegmentRepository;
    private readonly IBusStopRepository _busStopRepository;

    public RouteSegmentController(IRouteSegmentRepository routeSegmentRepository, IBusStopRepository busStopRepository)
    {
        _routeSegmentRepository = routeSegmentRepository;
        _busStopRepository = busStopRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddRouteSegmentRequestDto addRouteSegmentRequestDto)
    {
        var DepartureStop = await _busStopRepository.GetById(addRouteSegmentRequestDto.DepartureStopId);
        var ArrivalStop = await _busStopRepository.GetById(addRouteSegmentRequestDto.ArrivalStopId);
        if (DepartureStop == null || ArrivalStop == null)
        {
            return NotFound("Bus stop not found");
        }

        var routeSegment = new RouteSegment()
        {
            DepartureStopId = addRouteSegmentRequestDto.DepartureStopId,
            ArrivalStopId = addRouteSegmentRequestDto.ArrivalStopId,
            Distance = addRouteSegmentRequestDto.Distance,
            Duration = TimeSpan.FromMinutes(double.Parse(addRouteSegmentRequestDto.Duration))
        };
        var createSegment = await _routeSegmentRepository.CreateAsync(routeSegment);
        return Ok(createSegment);
    }
}