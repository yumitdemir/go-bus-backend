using go_bus_backend.Data;
using go_bus_backend.Dto.BusStop;
using go_bus_backend.Dto.Driver;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using go_bus_backend.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace go_bus_backend.Controllers;

[Route("api/[controller]")]
public class BusStopController : ControllerBase
{
    private readonly IBusStopRepository _busStopRepository;

    public BusStopController(IBusStopRepository busStopRepository)
    {
        _busStopRepository = busStopRepository;
    }


    [HttpPost]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> Create([FromBody] AddBusStopRequestDto addBusStopRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var busStop = new BusStop()
        {
            Name = addBusStopRequestDto.Name,
            City = addBusStopRequestDto.City,
            Address = addBusStopRequestDto.Address,
            Country = addBusStopRequestDto.Country
        };
        var createdBusStop = await _busStopRepository.CreateAsync(busStop);
        return Ok(createdBusStop);
    }

    [HttpPut]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult?> Update(int id, [FromBody] UpdateBusStopRequestDto updateBusStopRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var oldBusStop = await _busStopRepository.GetById(id);
        if (oldBusStop == null)
            NotFound();

        var busStop = new BusStop()
        {
            Name = updateBusStopRequestDto.Name,
            City = updateBusStopRequestDto.City,
            Address = updateBusStopRequestDto.Address,
            Country = updateBusStopRequestDto.Country
        };
        var updatedBusStop = await _busStopRepository.UpdateAsync(id, busStop);
        return Ok(updatedBusStop);
    }

    [HttpDelete]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> Delete([FromBody] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        var busstop = await _busStopRepository.GetById(id);
        if (busstop == null)
            return NotFound();


        var isBusStopInUse = await _busStopRepository.IsBusStopInUse(id);
        if (isBusStopInUse)
            return BadRequest("Bus stop is currently in use and cannot be deleted");
        var deletedBusStop = await _busStopRepository.DeleteAsync(id);
        return Ok(deletedBusStop);
    }

    [HttpGet]
    [Route("GetAllBusStops")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> GetAllBusStops()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var busStops = await _busStopRepository.GetAllWithoutFilterAsync();
        return Ok(busStops);
    }


    [HttpGet]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetBusStops([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var busStops = await _busStopRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, page,
            pageSize);
        return Ok(busStops);
    }

    [HttpGet("GetById")]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var bus = await _busStopRepository.GetById(id);
        return Ok(bus);
    }
}