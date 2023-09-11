using go_bus_backend.Dto.Driver;
using go_bus_backend.Models;

namespace go_bus_backend.Interfaces;

public interface IDriverRepository
{
    public Task<DriverGetAllAsyncDto> GetAllAsync(string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true, int page = 1, int pageSize = 10);

    public Task<Driver?> DeleteAsync(int id);
    public Task<Driver?> UpdateAsync(int id, Driver driver);
    public Task<Driver> CreateAsync(Driver driver);
    public Task<Driver?> GetById(int id);
}