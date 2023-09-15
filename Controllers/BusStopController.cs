using go_bus_backend.Data;
using go_bus_backend.Dto.BusStop;
using go_bus_backend.Dto.Driver;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using go_bus_backend.Repository;
using Microsoft.AspNetCore.Mvc;

namespace go_bus_backend.Controllers;

[Route("api/[controller]")]
public class BusStopController : ControllerBase
{
    private readonly IBusStopRepository _busStopRepository;

    public BusStopController(IBusStopRepository busStopRepository )
    {
        _busStopRepository = busStopRepository;
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddBusStopRequestDto addBusStopRequestDto)
    {
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
    public async Task<IActionResult?> Update(int id, [FromBody] UpdateBusStopRequestDto updateBusStopRequestDto)
    {
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
    public async Task<IActionResult> Delete([FromBody] int id)
    {
        var deletedBusStop = await _busStopRepository.DeleteAsync(id);
        return Ok(deletedBusStop);
    }

    [HttpGet]
    public async Task<IActionResult> GetBusStops([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var busStops = await _busStopRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, page,
            pageSize);
        return Ok(busStops);
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(int id)
    {
        var bus = await _busStopRepository.GetById(id);
        return Ok(bus);
    }
}