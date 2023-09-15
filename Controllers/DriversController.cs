﻿using go_bus_backend.Dto.Driver;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace go_bus_backend.Controllers;

[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly IDriverRepository _driverRepository;
  

    public DriversController(IDriverRepository driverRepository)
    {
        _driverRepository = driverRepository;
       
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddDriverRequestDto addDriverRequestDto)
    {
        var driver = new Driver()
        {
            Name = addDriverRequestDto.Name,
            Surname = addDriverRequestDto.Surname,
            LicenseNumber = addDriverRequestDto.LicenseNumber,
            DateOfBirth = addDriverRequestDto.DateOfBirth,
            DriverStatus = bool.Parse(addDriverRequestDto.DriverStatus),
            ContactNumber = addDriverRequestDto.ContactNumber,
            
        };
        var createdDriver = await _driverRepository.CreateAsync(driver);
        return Ok(createdDriver);
    }

    [HttpPut]
    public async Task<IActionResult?> Update(int id, [FromBody] UpdateDriverRequestDto updateDriverRequestDto)
    {
        
        
        var bus = new Driver()
        {
            Name = updateDriverRequestDto.Name,
            Surname = updateDriverRequestDto.Surname,
            LicenseNumber = updateDriverRequestDto.LicenseNumber,
            DateOfBirth = updateDriverRequestDto.DateOfBirth,
            DriverStatus = updateDriverRequestDto.DriverStatus,
            ContactNumber = updateDriverRequestDto.ContactNumber,

        };
        var updatedBus = await _driverRepository.UpdateAsync(id, bus);
        return Ok(updatedBus);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] int id)
    {
        var deletedDriver = await _driverRepository.DeleteAsync(id);
        return Ok(deletedDriver);
    }

    [HttpGet]
    public async Task<IActionResult> GetDrivers([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var drivers = await _driverRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, page,
            pageSize);
        return Ok(drivers);
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(int id)
    {
        var bus = await _driverRepository.GetById(id);
        return Ok(bus);
    }
}