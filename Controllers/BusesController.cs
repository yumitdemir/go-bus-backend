﻿using go_bus_backend.Dto;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace go_bus_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BusesController : ControllerBase
{
    private readonly IBusRepository _busRepository;

    public BusesController(IBusRepository busRepository)
    {
        _busRepository = busRepository;
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddBusRequestDto addBusRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var bus = new Bus()
        {
            Brand = addBusRequestDto.Brand,
            Capacity = addBusRequestDto.Capacity,
            Model = addBusRequestDto.Model,
            PlateNumber = addBusRequestDto.PlateNumber,
            LastMaintenanceDate = addBusRequestDto.LastMaintenanceDate,
            RestroomAvailable = bool.Parse(addBusRequestDto.RestroomAvailable),
            WiFiAvailable = bool.Parse(addBusRequestDto.WiFiAvailable)
        };
        var createdBus = await _busRepository.CreateAsync(bus);
        return Ok(createdBus);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBusRequestDto updateBusRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var oldBus = await _busRepository.GetById(id);
        if (oldBus == null)
            return NotFound();
        
        var bus = new Bus()
        {
            Brand = updateBusRequestDto.Brand,
            Capacity = updateBusRequestDto.Capacity,
            Model = updateBusRequestDto.Model,
            PlateNumber = updateBusRequestDto.PlateNumber,
            LastMaintenanceDate = updateBusRequestDto.LastMaintenanceDate,
            RestroomAvailable = bool.Parse(updateBusRequestDto.RestroomAvailable),
            WiFiAvailable = bool.Parse(updateBusRequestDto.WiFiAvailable)
        };
        var updatedBus = await _busRepository.UpdateAsync(id, bus);
        return Ok(updatedBus);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var bus = await _busRepository.GetById(id);
        if (bus == null)
            return NotFound();

        var isBusInUse = await _busRepository.IsBusInUse(id);
        if (isBusInUse)
        {
            return BadRequest("Bus is currently in use and cannot be deleted");
        }
        var deletedBus = await _busRepository.DeleteAsync(id);
        return Ok(deletedBus);
    }

    [HttpGet]
    public async Task<IActionResult> GetBuses([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var buses = await _busRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, page,
            pageSize);
        return Ok(buses);
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var bus = await _busRepository.GetById(id);
        return Ok(bus);
    }
    [HttpGet("GetAllBuses")]
    public async Task<IActionResult> GetAllBuses()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var buses = await _busRepository.GetAllBuses();
        return Ok(buses);
    }
}