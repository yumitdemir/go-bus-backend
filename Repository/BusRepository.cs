using go_bus_backend.Data;
using go_bus_backend.Dto;
using go_bus_backend.Interfaces;
using go_bus_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace go_bus_backend.Repository;

public class BusRepository : IBusRepository
{
    private readonly DataContext _context;

    public BusRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<BusGetAllAsyncDto> GetAllAsync(string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true, int page = 1, int pageSize = 10)
    {
        var buses = _context.Buses.AsQueryable();
        // Filtering
        if (!string.IsNullOrWhiteSpace(filterQuery))
        {
            buses = buses.Where(x =>
                x.Brand.Contains(filterQuery) ||
                x.Model.Contains(filterQuery) ||
                x.PlateNumber.Contains(filterQuery) ||
                x.Capacity.ToString() == filterQuery
            );
        }

        // Sorting
        if (string.IsNullOrWhiteSpace(sortBy) == false)
        {
            if (sortBy.Equals("Brand", StringComparison.OrdinalIgnoreCase))
            {
                buses = isAscending ? buses.OrderBy(x => x.Brand) : buses.OrderByDescending(x => x.Brand);
            } else if (sortBy.Equals("Id", StringComparison.OrdinalIgnoreCase))
            {
                buses = isAscending ? buses.OrderBy(x => x.Id) : buses.OrderByDescending(x => x.Id);
            } else if (sortBy.Equals("Model", StringComparison.OrdinalIgnoreCase))
            {
                buses = isAscending ? buses.OrderBy(x => x.Model) : buses.OrderByDescending(x => x.Model);
            }else if (sortBy.Equals("Capacity", StringComparison.OrdinalIgnoreCase))
            {
                buses = isAscending ? buses.OrderBy(x => x.Capacity) : buses.OrderByDescending(x => x.Capacity);
            }
            else if (sortBy.Equals("Restroom Available", StringComparison.OrdinalIgnoreCase))
            {
                buses = isAscending ? buses.OrderBy(x => x.RestroomAvailable) : buses.OrderByDescending(x => x.RestroomAvailable);
            } else if (sortBy.Equals("WiFi Available", StringComparison.OrdinalIgnoreCase))
            {
                buses = isAscending ? buses.OrderBy(x => x.WiFiAvailable) : buses.OrderByDescending(x => x.WiFiAvailable);
            }
            else if (sortBy.Equals("Last Maintenance", StringComparison.OrdinalIgnoreCase))
            {
                buses = isAscending ? buses.OrderBy(x => x.LastMaintenanceDate) : buses.OrderByDescending(x => x.LastMaintenanceDate);
            }else if (sortBy.Equals("Plate Number", StringComparison.OrdinalIgnoreCase))
            {
                buses = isAscending ? buses.OrderBy(x => x.PlateNumber) : buses.OrderByDescending(x => x.PlateNumber);
            }
        }

        // Calculate the total count of buses (before pagination)
        int totalCount = await buses.CountAsync();

        // Pagination
        var skipResults = (page - 1) * pageSize;
        List<Bus> pagedBuses = await buses.Skip(skipResults).Take(pageSize).ToListAsync();

        // Calculate the biggest page number
        var biggestPageNumber = (int)Math.Ceiling((double)totalCount / pageSize);
        var getAllAsyncDto = new BusGetAllAsyncDto()
        {
            Buses = pagedBuses,
            BiggestPageNumber = biggestPageNumber
        };

        return getAllAsyncDto;
    }

    public async Task<Bus?> GetById(int id)
    {
        var bus = await _context.Buses.FirstOrDefaultAsync(x => x.Id == id);

        return bus;

    }

    public async Task<List<Bus>?> GetAllBuses()
        {
            var buses = await _context.Buses.ToListAsync();

            return buses;
        }


    public async Task<Bus> CreateAsync(Bus bus)
    {
        await _context.Buses.AddAsync(bus);
        await _context.SaveChangesAsync();
        return bus;
    }

    public async Task<Bus?> UpdateAsync(int id, Bus bus)
    {
        var existingBus = await _context.Buses.FirstOrDefaultAsync(x => x.Id == id);
        if (existingBus == null)
        {
            return null;
        }

        existingBus.Brand = bus.Brand;
        existingBus.Capacity = bus.Capacity;
        existingBus.Model = bus.Model;
        existingBus.RestroomAvailable = bus.RestroomAvailable;
        existingBus.LastMaintenanceDate = bus.LastMaintenanceDate;
        existingBus.WiFiAvailable = bus.WiFiAvailable;
        existingBus.PlateNumber = bus.PlateNumber;


        await _context.SaveChangesAsync();
        return existingBus;
    }

    public async Task<Bus?> DeleteAsync(int id)
    {
        var existingBus = await _context.Buses.FirstOrDefaultAsync(x => x.Id == id);
        if (existingBus == null)
        {
            return null;
        }

        _context.Remove(existingBus);
        await _context.SaveChangesAsync();
        return existingBus;
    }
}