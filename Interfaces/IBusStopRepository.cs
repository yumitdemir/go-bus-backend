using go_bus_backend.Dto.BusStop;
using go_bus_backend.Models;

namespace go_bus_backend.Interfaces;

public interface IBusStopRepository
{
    public Task<BusStopGetAllAsyncDto> GetAllAsync(string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true, int page = 1, int pageSize = 10);
    public Task<BusStop?> GetById(int id);
    public Task<BusStop> CreateAsync(BusStop busStop);
    public Task<BusStop?> UpdateAsync(int id, BusStop busStop);
    public Task<BusStop?> DeleteAsync(int id);
}