using go_bus_backend.Dto.BusStop;
using go_bus_backend.Dto.Route;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Route = go_bus_backend.Models.Route;

namespace go_bus_backend.Controllers;
[Route("api/[controller]")]
public class RouteController : ControllerBase
{
    private readonly IRouteRepository _routeRepository;
    private readonly IRouteSegmentRepository _routeSegmentRepository;
    private readonly IBusStopRepository _busStopRepository;

    public RouteController(IRouteRepository routeRepository, IRouteSegmentRepository routeSegmentRepository,IBusStopRepository busStopRepository)
    {
        _routeRepository = routeRepository;
        _routeSegmentRepository = routeSegmentRepository;
        _busStopRepository = busStopRepository;
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddRouteRequestDto addRouteRequestDto)
    {
        
        var routeSegments = new List<RouteSegment>();
        foreach (var id in addRouteRequestDto.RouteSegmentIds)
        {
            var routeSegment = await _routeSegmentRepository.GetByIdAsync(id);
            if (routeSegment == null)
            {
                return NotFound($"Route with ID {id} not found");
            }
            routeSegments.Add(routeSegment);
        }
        var busStops = new List<BusStop>();
        foreach (var id in addRouteRequestDto.BusStopIds)
        {
            var busStop = await _busStopRepository.GetById(id);
            if (busStop == null)
            {
                return NotFound($"BusStop with ID {id} not found");
            }
            busStops.Add(busStop);
        }
        var route = new Route()
        {
            RouteName = addRouteRequestDto.RouteName,
            RouteSegments = routeSegments,
            BusStops = busStops
        };
        var createdRoute = await _routeRepository.CreateAsync(route);
        return Ok(createdRoute);
    }

    [HttpPut]
    public async Task<IActionResult?> Update(int id, [FromBody] UpdateRouteRequestDto updateRouteRequestDto)
    {
        var route = new Route()
        {
            RouteName = updateRouteRequestDto.RouteName,
            RouteSegments = updateRouteRequestDto.RouteSegments,
            BusStops = updateRouteRequestDto.BusStops
        };
        var updatedRoute = await _routeRepository.UpdateAsync(id, route);
        return Ok(updatedRoute);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedRoute = await _routeRepository.DeleteAsync(id);
        return Ok(deletedRoute);
    }

    [HttpGet]
    public async Task<IActionResult> GetBusStops([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var routes = await _routeRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, page,
            pageSize);
        return Ok(routes);
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(int id)
    {
        var route = await _routeRepository.GetById(id);
        return Ok(route);
    }
}