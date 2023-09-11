using go_bus_backend.Data;
using go_bus_backend.Dto.Driver;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace go_bus_backend.Repository;

public class DriverRepository : IDriverRepository
{
    private readonly DataContext _context;

    public DriverRepository(DataContext context)
    {
        _context = context;
    }
    
     public async Task<DriverGetAllAsyncDto> GetAllAsync(string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true, int page = 1, int pageSize = 10)
    {
        var drivers = _context.Drivers.AsQueryable();
        
        // Filtering
        if (!string.IsNullOrWhiteSpace(filterQuery))
        {
            drivers = drivers.Where(x =>
                x.Name.Contains(filterQuery) ||
                x.Id.ToString() == filterQuery ||
                x.Surname.Contains(filterQuery) ||
                x.LicenseNumber.Contains(filterQuery) ||
                x.DateOfBirth.ToString().Contains(filterQuery) ||
                x.ContactNumber.Contains(filterQuery)
            );
        }

        // Sorting
        if (string.IsNullOrWhiteSpace(sortBy) == false)
        {
            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                drivers = isAscending ? drivers.OrderBy(x => x.Name) : drivers.OrderByDescending(x => x.Name);
            } else if (sortBy.Equals("Id", StringComparison.OrdinalIgnoreCase))
            {
                drivers = isAscending ? drivers.OrderBy(x => x.Id) : drivers.OrderByDescending(x => x.Id);
            } else if (sortBy.Equals("Surname", StringComparison.OrdinalIgnoreCase))
            {
                drivers = isAscending ? drivers.OrderBy(x => x.Surname) : drivers.OrderByDescending(x => x.Surname);
            }else if (sortBy.Equals("Contact Number", StringComparison.OrdinalIgnoreCase))
            {
                drivers = isAscending ? drivers.OrderBy(x => x.ContactNumber) : drivers.OrderByDescending(x => x.ContactNumber);
            }
            else if (sortBy.Equals("Status", StringComparison.OrdinalIgnoreCase))
            {
                drivers = isAscending ? drivers.OrderBy(x => x.DriverStatus) : drivers.OrderByDescending(x => x.DriverStatus);
            } else if (sortBy.Equals("License Number", StringComparison.OrdinalIgnoreCase))
            {
                drivers = isAscending ? drivers.OrderBy(x => x.LicenseNumber) : drivers.OrderByDescending(x => x.LicenseNumber);
            }
            else if (sortBy.Equals("Date Of Birth", StringComparison.OrdinalIgnoreCase))
            {
                drivers = isAscending ? drivers.OrderBy(x => x.DateOfBirth) : drivers.OrderByDescending(x => x.DateOfBirth);
            }
        }

        // Calculate the total count of buses (before pagination)
        int totalCount = await drivers.CountAsync();

        // Pagination
        var skipResults = (page - 1) * pageSize;
        List<Driver> pagedDrivers = await drivers.Skip(skipResults).Take(pageSize).ToListAsync();

        // Calculate the biggest page number
        var biggestPageNumber = (int)Math.Ceiling((double)totalCount / pageSize);
        var getAllAsyncDto = new DriverGetAllAsyncDto()
        {
            Drivers = pagedDrivers,
            BiggestPageNumber = biggestPageNumber
        };

        return getAllAsyncDto;
    }

    public async Task<Driver?> GetById(int id)
    {
        var driver = await _context.Drivers.FirstOrDefaultAsync(x => x.Id == id);

        return driver;
    }

    public async Task<Driver> CreateAsync(Driver driver)
    {
        await _context.Drivers.AddAsync(driver);
        await _context.SaveChangesAsync();
        return driver;
    }

    public async Task<Driver?> UpdateAsync(int id, Driver driver)
    {
        var existingDriver = await _context.Drivers.FirstOrDefaultAsync(x => x.Id == id);
        if (existingDriver == null)
        {
            return null;
        }

        existingDriver.Name = driver.Name;
        existingDriver.Surname = driver.Surname;
        existingDriver.DateOfBirth = driver.DateOfBirth;
        existingDriver.LicenseNumber = driver.LicenseNumber;
        existingDriver.ContactNumber = driver.ContactNumber;
        existingDriver.DriverStatus = driver.DriverStatus;


        await _context.SaveChangesAsync();
        return existingDriver;
    }

    public async Task<Driver?> DeleteAsync(int id)
    {
        var existingDriver = await _context.Drivers.FirstOrDefaultAsync(x => x.Id == id);
        if (existingDriver == null)
        {
            return null;
        }

        _context.Remove(existingDriver);
        await _context.SaveChangesAsync();
        return existingDriver;
    }
}