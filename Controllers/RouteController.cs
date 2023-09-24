using go_bus_backend.Dto.BusStop;
using go_bus_backend.Dto.Route;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using Microsoft.AspNetCore.Authorization;
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

    public RouteController(IRouteRepository routeRepository, IRouteSegmentRepository routeSegmentRepository,
        IBusStopRepository busStopRepository)
    {
        _routeRepository = routeRepository;
        _routeSegmentRepository = routeSegmentRepository;
        _busStopRepository = busStopRepository;
    }


    [HttpPost]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> Create([FromBody] AddRouteRequestDto addRouteRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        var routeSegments = new List<RouteSegment>();
        foreach (var routeSegment in addRouteRequestDto.RouteSegments)
        {
            var departureStop = await _busStopRepository.GetById(routeSegment.DepartureStopId);
            var arrivalStop = await _busStopRepository.GetById(routeSegment.ArrivalStopId);
            if (departureStop == null || arrivalStop == null)
            {
                return NotFound("Bus stop not found");
            }

            TimeSpan duration;
            try
            {
                duration = TimeSpan.FromMinutes(double.Parse(routeSegment.Duration));
            }
            catch (Exception)
            {
                return BadRequest("Invalid duration format");
            }

            var newRouteSegment = new RouteSegment()
            {
                DepartureStopId = routeSegment.DepartureStopId,
                ArrivalStopId = routeSegment.ArrivalStopId,
                Distance = routeSegment.Distance,
                Duration = duration
            };
            var createdSegment = await _routeSegmentRepository.CreateAsync(newRouteSegment);
            routeSegments.Add(createdSegment);
        }

        var route = new Route()
        {
            RouteName = addRouteRequestDto.RouteName,
            RouteSegments = routeSegments,
        };
        var createdRoute = await _routeRepository.CreateAsync(route);
        return Ok(createdRoute);
    }

    [HttpPut]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult?> Update(int id, [FromBody] UpdateRouteRequestDto updateRouteRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var oldRoute = await _routeRepository.GetById(id);
        if (oldRoute == null)
            return NotFound();

        var routeSegments = new List<RouteSegment>();
        foreach (var segmentId in updateRouteRequestDto.RouteSegmentIds)
        {
            var routeSegment = await _routeSegmentRepository.GetByIdAsync(segmentId);
            if (routeSegment != null) routeSegments.Add(routeSegment);
        }

        var route = new Route()
        {
            RouteName = updateRouteRequestDto.RouteName,
            RouteSegments = routeSegments,
        };
        var updatedRoute = await _routeRepository.UpdateAsync(id, route);
        return Ok(updatedRoute);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var route = await _routeSegmentRepository.GetByIdAsync(id);
        if (route == null)
            return NotFound();

        var isRouteInUse = await _routeRepository.IsRouteInUse(id);
        if (isRouteInUse)
        {
            return BadRequest("Route is currently in use and cannot be deleted");
        }

        var deletedRoute = await _routeRepository.DeleteAsync(id);
        return Ok(deletedRoute);
    }

    [HttpGet]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetRoutes([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        var routes = await _routeRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, page,
            pageSize);
        return Ok(routes);
    }

    [HttpGet("GetById")]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var route = await _routeRepository.GetById(id);
        return Ok(route);
    }

    [HttpGet("GetAllRoutes")]
    [Authorize(Roles = "Reader,Writer")]
    public async Task<IActionResult> GetAllRoutes()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var routes = await _routeRepository.GetAllRoutes();
        return Ok(routes);
    }
}